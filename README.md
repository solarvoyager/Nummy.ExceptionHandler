# Nummy Global Exception & Logging Handling, Monitoring package for .NET Core

[![NuGet Version](https://img.shields.io/nuget/v/Nummy.ExceptionHandler.svg)](https://www.nuget.org/packages/Nummy.ExceptionHandler/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Overview

This is a .NET Core library for global exception handling in your application.

## Installation

https://www.nuget.org/packages/Nummy.ExceptionHandler
Or install the package via NuGet Package Manager Console:

```bash
Install-Package Nummy.ExceptionHandler
```

## Getting Started

In your `Program.cs` file add the following line:

```csharp
using Nummy.ExceptionHandler.Extensions;
using Nummy.ExceptionHandler.Models;
```

```csharp
// .. other configurations

builder.Services.AddNummyExceptionHandler(options =>
{
    // Configure options here
    // Example: 
    options.ReturnResponseDuringException = true;                 // if false, the app throws exceptions as a normal
    options.ResponseContentType = NummyResponseContentType.Json;
    options.ResponseStatusCode = HttpStatusCode.BadRequest;
    options.Response = new { message = "An error occurred" };     // or your custom object
});

// .. other configurations
var app = builder.Build();
```

```csharp
var app = builder.Build();

// .. other configurations

app.UseNummyExceptionHandler();

// .. other middleware
```

Now, your application is set up to handle unhandled exceptions globally using the Nummy Exception Handler.

## License

This library is licensed under the MIT License.