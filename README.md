# PulseLogic HMIS v2.0.1

PulseLogic Health Management Information System (HMIS) is a comprehensive, N-Tier enterprise health management platform. Designed for hospitals and clinics, it bridges clinical care, administrative management, pharmacy operations, and billing into a unified, high-performance web application.

## 🚀 Key Features

* **Clinical & Nursing Management**: Complete patient history, clinical notes, charting, and vitals tracking.
* **Pharmacy & Dispensary**: Supplier ledgers, purchase orders, inventory tracking, and dynamic rack management.
* **Patient Appointments & Scheduling**: Queue management, doctor scheduling, and patient registration.
* **Billing & Accounting**: Automated invoicing, receipt generation (including localized formats like Nepali Receipt), and insurance/claims management.
* **Robust Security Engine**: Custom Role-Based Access Control (RBAC), end-to-end JWT Authentication, and an immutable SQL Server auditing ledger.
* **Extensibility**: Support for ABDM, FHIR integrations, and telemedicine modules.

## 🛠️ Technology Stack

* **Frontend**: Angular Single Page Application (SPA), dynamically served from the `.NET` middleware, built with a component-based UI and lazy-loaded modules.
* **Backend API**: ASP.NET Core Web API (C#) driving modular controllers, strict dependency injection, and centralized service layers.
* **Data Access**: Entity Framework Core & ADO.NET communicating with SQL Server.
* **Performance**: Distributed SQL caching, memory caching, and response compression for rapid API delivery.

## 📐 Architecture Documentation

Detailed documentation explaining the system design can be found in the `architecture/` directory:

1. [System Overview](architecture/01_System_Overview.md)
2. [Frontend Architecture](architecture/02_Frontend_Architecture.md)
3. [Backend API Architecture](architecture/03_Backend_API_Architecture.md)
4. [Data Access and Core](architecture/04_Data_Access_and_Core.md)
5. [Security and Cross-Cutting Concerns](architecture/05_Security_and_CrossCutting.md)

## ⚙️ Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download) (or matching target framework SDK)
* [Node.js](https://nodejs.org/) & Angular CLI (for frontend modifications)
* SQL Server (LocalDB or dedicated instance)

### Setup & Run
1. **Database Configuration**: 
   Ensure your SQL Server instance is running. Update the encrypted `Connectionstring` and `ConnectionStringAdmin` within `appsettings.json` (or your local environment secrets).
2. **Frontend Build** (Optional if already compiled):
   Navigate to `wwwroot/PulseLogicApp` and run `npm install`, followed by `ng build` to output the static files to `wwwroot/PulseLogicApp/dist`.
3. **Backend Run**:
   Open the solution in Visual Studio or run from the CLI via:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```
4. **Access the App**: Navigate to `http://localhost:<port>` to view the Angular application. Swagger API documentation is available at `/swagger` when running in the Development environment.

## 🔒 Security & Compliance
This system integrates HIPAA-compliant tracking. All HTTP requests are wrapped by an `ExceptionHandlingMiddleware` preventing stack-trace leaks, and structural changes to data are logged via `Audit.SqlServer.Providers`. Ensure `IsAuditEnable` is set to `true` in production configurations.

---

*Documentation updated for v2.0.1. See `PATCH_NOTES_v2.0.1.md` for recent architectural updates.*
