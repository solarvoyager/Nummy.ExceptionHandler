# Nummy.ExceptionHandler - Improvement Plan

## Package Summary
Global exception handling middleware for ASP.NET Core. Catches unhandled exceptions, logs them to the remote Nummy service, and returns a configurable error response to the client instead of crashing or leaking stack traces.

---

## Critical Issues

### 1. Uses `Microsoft.NET.Sdk.Web` — ships an entire web host with the library
- **File:** `Nummy.ExceptionHandler.csproj`
- **Problem:** The project uses `<Project Sdk="Microsoft.NET.Sdk.Web">` instead of `Microsoft.NET.Sdk`. This causes the NuGet package to pull in the entire ASP.NET Core web hosting stack as dependencies. Additionally, it generates a `Program.cs` entry point (which exists in the project as a commented-out stub with `Console.WriteLine("Hello world")`). A library should **never** use `Sdk.Web` — it should use `Microsoft.NET.Sdk` and reference `Microsoft.AspNetCore.App` as a `FrameworkReference`.
- **Fix:**
  ```xml
  <Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
      <TargetFramework>net9.0</TargetFramework>
    </PropertyGroup>
    <ItemGroup>
      <FrameworkReference Include="Microsoft.AspNetCore.App" />
    </ItemGroup>
  </Project>
  ```
  Delete `Program.cs` entirely.

### 2. Logging failure in exception handler can cause silent 500 with no response body
- **File:** `Middlewares/NummyExceptionMiddleware.cs:24-25`
- **Problem:** If `loggerService.LogAsync()` throws (Nummy service down, network issue, timeout), the exception from logging will propagate up **instead of the original exception**. The `HandleExceptionAsync` call on line 25 never executes, so the client gets a raw 500 with no body and the original exception is lost.
- **Fix:** Wrap the logging call in try/catch:
  ```csharp
  catch (Exception exception)
  {
      if (!options.Value.HandleException) throw;

      try
      {
          await loggerService.LogAsync(NummyCodeLogLevel.Error, exception);
      }
      catch
      {
          // Logging must never prevent exception handling
      }

      await HandleExceptionAsync(context);
  }
  ```

### 3. HttpClient created from Singleton holds stale connections
- **File:** `Data/Services/NummyCodeLoggerService.cs:13`
- **Problem:** Same pattern as the other packages. Singleton service caches `HttpClient` from factory, bypassing handler rotation and DNS refresh.
- **Fix:** Create client per call.

### 4. No exception handling in `InsertLogAsync`
- **File:** `Data/Services/NummyCodeLoggerService.cs:34`
- **Problem:** `PostAsJsonAsync` can throw on network failures. Since this is called from within the exception middleware's catch block, a failure here replaces the original exception (see #2).
- **Fix:** Wrap in try/catch.

### 5. Response body may already be started when exception occurs
- **File:** `Middlewares/NummyExceptionMiddleware.cs:29-33`
- **Problem:** `HandleExceptionAsync` sets `context.Response.StatusCode` and calls `WriteAsJsonAsync`. If the response has already started (headers sent, e.g., during streaming or chunked transfer), this will throw `InvalidOperationException: "Response has already started"`. This second exception replaces the original one.
- **Fix:** Check `context.Response.HasStarted`:
  ```csharp
  private async Task HandleExceptionAsync(HttpContext context)
  {
      if (context.Response.HasStarted) return;

      context.Response.StatusCode = (int)options.Value.ResponseStatusCode;
      await context.Response.WriteAsJsonAsync(options.Value.Response);
  }
  ```

---

## Performance Issues

### 6. 30-second HTTP timeout blocks the exception response
- **File:** `Extensions/NummyExceptionServiceExtension.cs:27`
- **Problem:** When an exception occurs and the Nummy service is slow/down, the user waits up to 30 seconds for the logging call before getting the error response. This is terrible UX for error scenarios.
- **Fix:** Reduce timeout to 3-5 seconds, or better yet, fire-and-forget the logging and return the error response immediately.

### 7. Synchronous await on logging before returning error response
- **File:** `Middlewares/NummyExceptionMiddleware.cs:24-25`
- **Problem:** The middleware awaits the log call before sending the error response. The client is blocked waiting for the Nummy service round-trip.
- **Fix:** Fire-and-forget the logging, return error response immediately:
  ```csharp
  _ = loggerService.LogAsync(NummyCodeLogLevel.Error, exception);
  await HandleExceptionAsync(context);
  ```

---

## Thread Safety Issues

### 8. Singleton logger service caching HttpClient
- **File:** `Data/Services/NummyCodeLoggerService.cs:13`
- **Problem:** Same as other packages. Thread-safe for concurrent use but bypasses factory rotation.

---

## Reliability Issues

### 9. `HandleException = false` re-throws but doesn't log
- **File:** `Middlewares/NummyExceptionMiddleware.cs:22`
- **Problem:** When `HandleException` is false, the middleware just re-throws with `throw`. It does not log the exception to Nummy. This means users who set `HandleException = false` get no exception logging at all, making this package essentially useless for them.
- **Fix:** Always log, only optionally handle:
  ```csharp
  catch (Exception exception)
  {
      try { await loggerService.LogAsync(NummyCodeLogLevel.Error, exception); } catch { }

      if (!options.Value.HandleException) throw;
      await HandleExceptionAsync(context);
  }
  ```

### 10. Duplicate code — copies entities from Nummy.CodeLogger
- **File:** `Data/Entitites/NummyCodeLog.cs`, `Data/Entitites/NummyCodeLogLevel.cs`, `Data/Services/`
- **Problem:** The `NummyCodeLog`, `NummyCodeLogLevel`, `INummyCodeLoggerService`, and `NummyCodeLoggerService` are duplicated from `Nummy.CodeLogger`. If the API contract changes in one package, the other will be out of sync. This also means users who install both packages have two `NummyCodeLogLevel` enums and two `INummyCodeLoggerService` interfaces in different namespaces.
- **Fix:** Extract shared types into a `Nummy.Core` or `Nummy.Shared` package that both reference. Alternatively, have `Nummy.ExceptionHandler` depend on `Nummy.CodeLogger` and use its logging service.

### 11. `AddProblemDetails()` called unconditionally
- **File:** `Extensions/NummyExceptionServiceExtension.cs:20`
- **Problem:** `services.AddProblemDetails()` is called but never used — the middleware writes a custom response object, not a ProblemDetails response. This registers unnecessary services in the DI container. It may also conflict with the host app's own ProblemDetails configuration.
- **Fix:** Remove `services.AddProblemDetails()` since it's unused.

### 12. No `CancellationToken` propagation
- **File:** `Middlewares/NummyExceptionMiddleware.cs`, `Data/Services/NummyCodeLoggerService.cs`
- **Problem:** No cancellation token is passed to the HTTP logging call. If the client disconnects, the server still waits for the Nummy service call to complete.

### 13. `Program.cs` exists as a stub — should not be in a library
- **File:** `Program.cs`
- **Problem:** Contains `Console.WriteLine("Hello world")` with a commented-out web app template. This file exists because of the `Sdk.Web` usage. It should be removed entirely. Having an entry point in a library package is incorrect.
- **Fix:** Delete `Program.cs` and switch to `Microsoft.NET.Sdk` (see #1).

---

## Code Quality

### 14. Commented-out code
- **File:** `Extensions/NummyExceptionServiceExtension.cs:19`
- **Problem:** `//services.AddExceptionHandler<NummyExceptionHandler>();` is dead commented code.
- **Fix:** Remove it.

### 15. Typo in folder name `Entitites`
- **File:** `Data/Entitites/` directory
- **Problem:** "Entitites" should be "Entities".

### 16. Typo in validation exception messages
- **File:** `Utils/Exceptions/ApplicationIdValidationException.cs`
- **Problem:** "Make sure to it copied" — grammatically incorrect.
- **Fix:** "Make sure it is copied".
