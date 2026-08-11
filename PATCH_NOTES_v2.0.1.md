# PulseLogic HMR v2.0.1 - Feature and Patch Notes

## 🚀 Features

### Architecture Documentation Suite
Added a comprehensive suite of architecture documentation tailored for developers and stakeholders, detailing the system's N-Tier design.

* **System Overview (`01_System_Overview.md`)**: High-level interaction maps between the Angular frontend, ASP.NET Core Web API, and SQL databases.
* **Frontend Architecture (`02_Frontend_Architecture.md`)**: Comprehensive breakdown of the Angular SPA, including lazy-loaded modules (Clinical, Billing, Pharmacy), component structures, and state management.
* **Backend API Architecture (`03_Backend_API_Architecture.md`)**: In-depth explanation of the ASP.NET Core request pipeline, modular Controllers, Dependency Injection strategies, and the Service layer.
* **Data Access & Core (`04_Data_Access_and_Core.md`)**: Detailed the `DalLayer` and `ServerModel` structures, highlighting the Entity Framework Core Repository implementation.
* **Security & Cross-Cutting Concerns (`05_Security_and_CrossCutting.md`)**: Documentation on the custom Role-Based Access Control (RBAC) engine, JWT integrations, SQL Server auditing, and centralized error handling via `ExceptionHandlingMiddleware`.

## 🛠️ Patches & Minor Updates
* **Documentation Standardization**: All architecture markdown files have been uniformly structured and placed within the `architecture/` directory at the project root to ensure a clean source tree.
* **Mermaid Integration**: Added Mermaid.js diagram support in the System Overview for better visual topology representation.
