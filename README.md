# PulseLogic HMIS v2.0.1

PulseLogic Health Management Information System (HMIS) is a comprehensive, N-Tier enterprise health management platform. Designed for hospitals and clinics, it bridges clinical care, administrative management, pharmacy operations, and billing into a unified, high-performance web application.

---

## 🚀 Key Features

* **Clinical & Nursing Management**: Complete patient history, clinical notes, charting, and vitals tracking.
* **Pharmacy & Dispensary**: Supplier ledgers, purchase orders, inventory tracking, and dynamic rack management.
* **Patient Appointments & Scheduling**: Queue management, doctor scheduling, and patient registration.
* **Billing & Accounting**: Automated invoicing, receipt generation (including localized formats like Nepali Receipt), and insurance/claims management.
* **Robust Security Engine**: Custom Role-Based Access Control (RBAC), end-to-end JWT Authentication with strict production startup guards, and an immutable SQL Server auditing ledger.
* **API Rate Limiting**: Global sliding-window rate limiting (300 req/min) applied across all API endpoints.
* **Extensibility**: Support for ABDM, FHIR integrations, and telemedicine modules.

---

## 🛠️ Technology Stack

* **Frontend**: Angular Single Page Application (SPA), dynamically served from the `.NET` middleware, built with a component-based UI and lazy-loaded modules.
* **Backend API**: ASP.NET Core Web API (C#) driving modular controllers, strict dependency injection, and centralized service layers.
* **Data Access**: Entity Framework Core & ADO.NET communicating with SQL Server.
* **Performance**: Direct stream-based file uploads, distributed SQL caching, memory caching, and response compression for rapid API delivery.

---

## 📐 Architecture Documentation

Detailed documentation explaining the system design can be found in the `architecture/` directory:

1. [System Overview](architecture/01_System_Overview.md)
2. [Frontend Architecture](architecture/02_Frontend_Architecture.md)
3. [Backend API Architecture](architecture/03_Backend_API_Architecture.md)
4. [Data Access and Core](architecture/04_Data_Access_and_Core.md)
5. [Security and Cross-Cutting Concerns](architecture/05_Security_and_CrossCutting.md)

---

## 🔌 GitHub Model Context Protocol (MCP) Integration

This repository supports the **GitHub MCP Server**, enabling AI coding tools (Antigravity IDE, Cursor, Claude Desktop) to interact directly with `PulseLogicHMIS`.

### Quick MCP Configuration
Add the following to your MCP configuration (`.vscode/mcp.json` or IDE MCP settings):

```json
{
  "mcpServers": {
    "github": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-github"],
      "env": {
        "GITHUB_PERSONAL_ACCESS_TOKEN": "YOUR_GITHUB_PAT_HERE"
      }
    }
  }
}
```

*For complete step-by-step PAT creation and setup instructions across all IDEs, see [GITHUB_MCP_GUIDE.md](GITHUB_MCP_GUIDE.md).*

---

## ⚙️ Installation & Setup Guide

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [Node.js (v18+)](https://nodejs.org/) & Angular CLI
* SQL Server (LocalDB or dedicated instance)

### Installation & Execution Steps

1. **Clone Repository**:
   ```bash
   git clone https://github.com/anuragnarre/PulseLogicHMIS.git
   cd PulseLogicHMIS
   ```

2. **Configure Environment Secrets**:
   Set the production JWT environment key prior to launch:
   ```bash
   # Windows PowerShell
   $env:ASPNETCORE_JWT_KEY="YourSecureRandom256BitKeyHere"
   ```

3. **Database Configuration**:
   Update the connection strings in `appsettings.json` (`Connectionstring` and `ConnectionStringAdmin`) to point to your target SQL Server instance.

4. **Compile Frontend (Angular SPA)**:
   ```bash
   cd Code/Websites/PulseLogicHMR/wwwroot/PulseLogicApp
   npm install
   ng build --prod
   ```

5. **Build & Run Backend**:
   From the repository root:
   ```bash
   dotnet restore
   dotnet build
   dotnet run --project Code/Websites/PulseLogicHMR/PulseLogicHMR.csproj
   ```

6. **Access Application**:
   Navigate to `http://localhost:5000` (or configured port). Access Swagger API documentation at `/swagger` in Development mode.

---

## 🔒 Security & Compliance
This system integrates HIPAA-compliant tracking. All HTTP requests are wrapped by `ExceptionHandlingMiddleware` preventing stack-trace leaks, static asset uploads are restricted by MIME/extension whitelists, and database mutations are audited via `Audit.SqlServer.Providers`.

---

*Documentation & Patch Notes updated for v2.0.1. See [PATCH_NOTES_v2.0.1.md](PATCH_NOTES_v2.0.1.md) for full patch details.*
