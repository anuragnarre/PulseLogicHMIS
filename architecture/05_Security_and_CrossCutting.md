# Security & Cross-Cutting Architecture: PulseLogic HMR v2.0.1

## Overview
PulseLogic HMR treats security, fault tolerance, and application configuration as first-class citizens. These cross-cutting concerns are designed to intercept requests globally before they hit domain-specific logic.

## 1. Security Architecture

### Role-Based Access Control (RBAC)
The application employs a custom RBAC engine housed within `PulseLogicHMR.Security`. 
- **Initialization**: Configured as a Singleton service during startup (`services.AddSingleton<RBAC>`).
- **PulseLogicActionFilter**: An ASP.NET Core Action Filter that runs before every controller action. It intercepts the HTTP request, identifies the requested route, checks the authenticated user's permissions, and determines whether to allow or deny the execution of the API.
- **Client-Side RBAC**: The UI uses route guards and structural directives in Angular to hide/show navigation menus or disable buttons based on the roles provided in the JWT token.

### JWT Authentication
- The system generates standard JSON Web Tokens (JWT) upon successful login in the `AccountController`.
- All subsequent API requests from the Angular application must carry the `Authorization: Bearer <Token>` header.
- Token validation is handled by the `Microsoft.AspNetCore.Authentication.JwtBearer` middleware.

### Credential Protection
- Database connection strings and sensitive passwords within the `appsettings.json` are encrypted.
- A custom decryption routine (`DecryptPassword` via RBAC) unpacks these connection strings in memory dynamically when `Startup.cs` runs, preventing credentials from being stored in plaintext within source control or config files.

## 2. Fault Tolerance and Error Handling

### Exception Handling Middleware
Instead of scattering `try-catch` blocks across all controllers, a centralized `ExceptionHandlingMiddleware` wraps the entire HTTP pipeline.
- It catches unhandled exceptions thrown by any downstream service or DAL code.
- It prevents stack traces from bleeding to the client.
- It translates exceptions into standardized HTTP 500 JSON responses.

### Rate Limiting
To protect against Denial of Service (DoS) and brute force attacks, `.NET 8` native rate limiting is configured in `Startup.cs`:
- **Login Policy**: Strict limits on login attempts to thwart brute force password guessing (e.g., 10 attempts per minute).
- **API General Policy**: A sliding window limit applied globally to ensure stable server performance even under heavy load.

## 3. Configuration and Hosting Context
- **Environment Parity**: The application supports dynamic behavior switches between `Development` and `Production` (e.g., `SecurePolicy` on session cookies, HSTS headers).
- **Swagger Documentation**: Kept strictly behind the `env.IsDevelopment()` flag to prevent exposing the API surface in production.
