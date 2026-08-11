# Frontend Architecture: PulseLogic HMR v2.0.1

## Overview
The frontend of PulseLogic HMR is built as a Single Page Application (SPA) using Angular. It resides in `wwwroot/PulseLogicApp/src/app` and is configured to be served directly via the ASP.NET Core middleware (`app.UseFileServer()` and route mappings in `Startup.cs`). 

## Core Design Principles

### 1. Modular Architecture
The Angular application uses a heavily modularized approach (feature modules). Instead of a single monolithic application module, the system is divided into logical domain areas mapping to the hospital's operational needs.

Key modules include:
- `appointments`: Patient scheduling and appointment management.
- `billing`: Patient invoicing, settlements, and receipt generation.
- `clinical`: Clinical notes, doctor interactions, and patient histories.
- `pharmacy`: Dispensary, inventory, and supplier ledgers.
- `nursing`, `inventory`, `maternity`, `radiology`, `labs`, etc.

This modularity enables:
- **Lazy Loading**: Only the code required for a specific hospital module is downloaded to the browser when the user navigates to it, improving initial load times.
- **Separation of Concerns**: Teams can work on separate modules without creating merge conflicts in a central file.

### 2. Component-Based UI
Each module is composed of smart and presentation components. For example, `supplier-ledger-view.component.html` in the Pharmacy module acts as a view that binds to a TypeScript class (`.ts`) handling the UI logic, user input, and displaying data fetched from the backend.

### 3. Services and Dependency Injection
Angular Services (`@Injectable`) are used to decouple HTTP communication and state management from components.
- **HTTP Clients**: Services interact with the ASP.NET Core backend APIs using Angular's `HttpClient`.
- **State Management**: Data that needs to be shared across components (e.g., currently selected patient, user session details) is managed within services.

### 4. Routing
The application utilizes Angular's `RouterModule`. Route definitions are likely centralized (e.g., `app-routing.constant.ts` and module-specific routing files) to map URLs to specific components.
The backend MVC setup in `Startup.cs` redirects fallback routes to `Home/Index`, which serves the `index.html` of the Angular SPA, allowing Angular to handle client-side routing.

### 5. Utilities and Shared Resources
- **Shared Module**: Contains reusable UI components, pipes, and directives used across multiple feature modules.
- **Core Module**: Contains singleton services like Authentication, Authorization (client-side RBAC), and Error Interceptors.

## Security Integration
Client-side security relies on tokens (JWT) provided by the backend upon login.
1. The user authenticates via a login component.
2. The backend returns a token.
3. An `HttpInterceptor` in Angular attaches this token to the `Authorization: Bearer` header for all subsequent API requests.
4. Route Guards (`CanActivate`) prevent unauthorized access to modules based on the user's roles (RBAC).
