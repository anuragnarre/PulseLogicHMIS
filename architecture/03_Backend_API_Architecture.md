# Backend API Architecture: PulseLogic HMR v2.0.1

## Overview
The backend application is an ASP.NET Core MVC/Web API project. It provides the necessary endpoints for the Angular frontend, handles business rules, and communicates with the data layer. 

## Request Pipeline and Middleware
The HTTP request pipeline is configured in `Startup.cs` (`Configure` method) and processes incoming requests sequentially through middleware:
1. **Security Headers**: Custom middleware (`SecurityHeadersMiddleware`) injects essential security headers (anti-clickjacking, HSTS).
2. **Response Compression**: Compresses JSON payloads to reduce bandwidth.
3. **Exception Handling**: Global `ExceptionHandlingMiddleware` captures unhandled exceptions, logs them securely, and returns standardized error responses to prevent information leakage.
4. **Rate Limiting**: Built-in `.NET 8` rate limiting prevents brute force attacks (e.g., 10 attempts/minute on login) and API abuse.
5. **Authentication & Session**: Validates JWTs and manages session states.
6. **MVC Routing**: Maps incoming URLs to corresponding API controllers.

## API Structure

### 1. Controllers
Controllers (located in `Controllers/`) are modularized similarly to the frontend (e.g., `AccountController.cs`, `Billing/`, `Pharmacy/`). 
- They handle HTTP GET/POST/PUT/DELETE requests.
- They utilize Data Annotations to validate incoming ViewModels/DTOs.
- They use the `[PulseLogicActionFilter]` for deep authorization checks based on the user's role.

### 2. Service Layer (Business Logic)
Controllers remain thin; they do not contain complex business logic or direct database queries. Instead, they rely on the Service Layer (located in `Services/`).
- **Domain Services**: E.g., `PharmacyPO` (Purchase Orders), `QueueManagement`, `Maternity`.
- **Utility Services**: E.g., `EmailService`, `FileUploader`.

### 3. Dependency Injection (DI)
ASP.NET Core's built-in IoC (Inversion of Control) container manages the instantiation of services.
- Services are heavily registered via extension methods (e.g., `services.AddPulseLogicServices(Configuration)` in `PulseLogicServicesExtensions.cs`).
- Singletons are used for stateless or application-wide components (e.g., `PulseLogicCache`, `RBAC`, `NepaliDate`).
- Scoped/Transient services are used for request-specific operations like database interactions.

## Caching Strategy
To ensure high performance in a health management setting, caching is extensively utilized:
- **`PulseLogicCache`**: A custom caching wrapper.
- **SQL Server Distributed Cache / Memory Cache**: Depending on the environment, configuration such as `CacheExpirationMinutes` dictates how long RBAC rules and master data are retained before hitting the database again.
