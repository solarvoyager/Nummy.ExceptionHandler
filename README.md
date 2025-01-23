# Nummy Global Exception & Logging Handling, Monitoring package for .NET Core

[![NuGet Version](https://img.shields.io/nuget/v/Nummy.ExceptionHandler.svg)](https://www.nuget.org/packages/Nummy.ExceptionHandler/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Nummy.ExceptionHandler.svg)](https://www.nuget.org/packages/Nummy.ExceptionHandler/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## Overview

This is a .NET Core library for global exception handling in your application.

## Installation

[Nuget - Nummy.ExceptionHandler](https://www.nuget.org/packages/Nummy.ExceptionHandler)

or install the package via NuGet Package Manager Console:

```bash
Install-Package Nummy.ExceptionHandler
```

## Getting Started

#### 1. Run Nummy on your Docker and get DSN url of your local instance

[Here is tutorial](https://github.com/solarvoyager/Nummy/blob/fb5247f0b977d1d20424abc4c87f8a1c0d621bcd/README.md)

#### 2. Configure your application

In your `Program.cs` file add the following line:

```csharp
using Nummy.ExceptionHandler.Extensions;
using Nummy.ExceptionHandler.Models;
```

```csharp
// .. other configurations

builder.Services.AddNummyExceptionHandler(options =>
{
    // if false, the app throws exceptions as a normal
    options.HandleException = true;  
    // set your response object and status code
    options.Response = new { message = "An error occurred" };
    options.ResponseStatusCode = HttpStatusCode.BadRequest;
    options.DsnUrl = "your-nummy-dsn-url"
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

> **Attetion:** if you are using [Nummy.HttpLogger](https://www.nuget.org/packages/Nummy.HttpLogger),
> make sure to first register NummyHttpLogger and then NummyExceptionHandler.

#### 3. Now, your application is set up to handle unhandled exceptions globally using the Nummy Exception Handler.

## License

This library is licensed under the MIT License.