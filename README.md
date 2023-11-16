
# Nummy Global Exception & Logging Handling, Monitoring package for .NET Core

[![NuGet Version](https://img.shields.io/nuget/v/Nummy.ExceptionHandler.svg)](https://www.nuget.org/packages/Nummy.ExceptionHandler/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Overview

This is a .NET Core library for global exception handling in your application. It provides a centralized mechanism to handle unhandled exceptions and log them appropriately.

### Features

- **Global Exception Handling**: Capture unhandled exceptions at the application level.
- **Customizable**: Implement custom handlers for specific exception types.
- **NuGet Package**: Easily install via NuGet.

## Installation

https://www.nuget.org/packages/Nummy.ExceptionHandler
Or install the package via NuGet Package Manager Console:

```bash
Install-Package Nummy.ExceptionHandler
```

## Getting Started
In your `Program.cs` file add the following line to register the Nummy Exception Handler:

```csharp
using Nummy.ExceptionHandler.Extensions;
```
```csharp
// ... other configurations

services.AddNummyExceptionHandler(options =>
{
    // Configure Nummy Exception Handler options here, if needed
    // Example: options.IncludeExceptionDetails = true;
});

// ... other configurations
var app = builder.Build();
```
Use Middleware: In the Configure method of your Startup.cs file, add the following line to use the Nummy Exception Handler middleware:

```csharp
using Nummy.ExceptionHandler.Extensions;
```
```csharp
// ... other configurations

app.UseNummyExceptionHandler();

// ... other middleware
```
Now, your application is set up to handle unhandled exceptions globally using the Nummy Exception Handler.

## Customization
If you want to customize the behavior of the Nummy Exception Handler, you can pass additional options when configuring the services. For example:

```csharp
builder.Services.AddNummyExceptionHandler(options =>
{
    options.ReturnResponseDuringException = true;
    options.ResponseStatusCode = HttpStatusCode.BadRequest;
    options.Response = new YourResultModel("General Error!");
});
```
This allows you to tailor the exception handling to fit the specific needs of your application.

## License
This library is licensed under the MIT License.