# Middlewares for ASP.NET Core Project

This project contains custom middleware components and extensions designed to improve the security, error handling, and validation in an ASP.NET Core application.

---

## Folder Structure

- **Extensions**  
  Contains extension methods that help easily add middleware to the ASP.NET Core pipeline.  
  Example: `SecurityMiddleware` sets important HTTP security headers such as `X-Content-Type-Options`, `X-Frame-Options`, and others.

- **Handlers**  
  Contains middleware classes for specific tasks like global exception handling and model validation.  
  Examples:  
  - `GlobalExceptionHandlingMiddleware` handles uncaught exceptions globally and returns consistent JSON error responses.  
  - `ValidateModelAttribute` an action filter attribute that checks model validity before executing controller actions and returns detailed validation errors.

---

## How to Use

### Register Middlewares in `Program.cs` or `Startup.cs`

```csharp
app.UseSecureHeaders();
app.UseStrictTransportSecurity();
app.UsePermissionPolicy();
app.UseCrossOriginPolicy();
app.UseRequestSizeLimit(1024 * 1024); // 1 MB request size limit

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
