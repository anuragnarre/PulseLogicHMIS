# Data Access & Core Architecture: PulseLogic HMR v2.0.1

## Overview
The Data Access and Domain logic in PulseLogic HMR are abstracted into distinct component libraries: `PulseLogicHMR.DalLayer`, `PulseLogicHMR.ServerModel`, and `PulseLogicHMR.Core`. This strict separation ensures that the presentation and API layers do not interact with the database directly.

## `PulseLogicHMR.ServerModel` (Domain Entities)
This library contains the pure C# POCOs (Plain Old CLR Objects) that map directly to the database tables. 
- **Structure**: Includes models for Master data (Countries, Employees), clinical data (ICD10 Codes), and module-specific entities.
- **Responsibility**: Represents the structure of the data as it exists in SQL Server.

## `PulseLogicHMR.DalLayer` (Data Access Layer)
This library acts as the conduit between the Application Services and the SQL Server databases.

### 1. Database Contexts
The application utilizes a `DbContext` (e.g., `MasterDbContext`) leveraging Entity Framework (EF) Core for object-relational mapping.
- **Master Database**: Handles all transactional data (Patients, Bills, Appointments, Clinical Records).

### 2. Connection String Management
Connection strings are securely managed in `appsettings.json` and are dynamically decrypted at runtime in `Startup.cs`. The application maintains connections to both the Master DB and an Admin/Audit DB.

### 3. Repository Pattern Implementation
While EF Core implements a Repository/Unit of Work pattern by default, the `DalLayer` abstracts specific LINQ queries and complex stored procedure calls into reusable repository classes. This allows the backend Services to simply call methods like `_pharmacyRepository.GetSupplierLedger(id)`.

## `PulseLogicHMR.Core` (Shared Utilities)
This library holds cross-cutting application logic that is not specific to API controllers or UI.
- **Caching Logic**: Implements the core logic for the distributed caching mechanisms.
- **Helpers & Extensions**: Provides reusable methods utilized by both the API and the DalLayer.

## Auditing and Tracking
The system integrates `Audit.SqlServer.Providers`.
- **Purpose**: To maintain strict HIPAA/compliance logs.
- **Mechanism**: Data changes triggered through the `DalLayer` are captured and serialized as JSON (`JsonColumnName = "Data"`) into a distinct `PulseLogicAudit` table in the Admin database, ensuring an immutable history of records.
