40# Implementation Plan: Backend & Data Architect (Dev 1)

This plan outlines the technical roadmap for the Backend and Data Architecture of the Billing and Stock Management application, following the defined agile sprints.

## Phase 0: Foundations & Infrastructure (Sprint 0)
**Goal**: Establish a robust and scalable architecture using EF Core.

- [ ] **Project Setup**:
    - Initialize the .NET project (Web API or integrated Blazor server).
    - Install necessary NuGet packages:
        - `Microsoft.EntityFrameworkCore`
        - `Microsoft.EntityFrameworkCore.Design`
        - `Microsoft.EntityFrameworkCore.SqlServer` (or intended provider).
- [ ] **Database Context**:
    - Create the `ApplicationDbContext`.
    - Configure the connection string in `appsettings.json`.
- [ ] **Dependency Injection**:
    - Register the DbContext in `Program.cs`.
- [ ] **Security & Resilience Setup**:
    - Add `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.
    - Configure **Identity Framework** (Users & Roles).
    - Implement a **Global Exception Middleware** to return standardized error responses.

## Phase 1: Core Domain Entities (Sprint 1)
**Goal**: Implement the primary master data entities.

- [ ] **Entity Modeling**:
    - Create `Client` entity with fiscal validation rules.
    - Create `Produit` entity with pricing and stock properties.
    - Implement `IBaseEntity` interface with `IsDeleted` property.
    - Configure **Global Query Filters** in `OnModelCreating` to automatically hide deleted records.
- [ ] **Database Migration**:
    - Add the initial migration.
    - Update the database to reflect the schema.
- [ ] **Data Access Layer**:
    - Implement `ProduitService` and `ClientService` for basic Fetch operations.
    - Implement a `SoftDelete` method in services instead of hard `Remove`.
    - Use the **Result Pattern** (`Result<T>`) for service responses to handle failures gracefully.

## Phase 2: Billing Engine (Sprint 2)
**Goal**: Implement the core billing logic and document relationships.

- [ ] **Billing Entities**:
    - Create `Facture` and `LigneFacture` entities.
    - Define relationships (Client -> Factures, Facture -> Lignes).
- [ ] **Billing Logic**:
    - Implement the `FactureService`.
    - Create methods for calculating **HT, TVA, and TTC** at both line and invoice levels.
    - Integrate `ParametreFiscal` for dynamic tax/stamp duty settings.
- [ ] **Validation**:
    - Ensure invoice numbers follow a specific format.

## Phase 3: Stock Management & Automation (Sprint 3)
**Goal**: Implement the inventory tracking system and business rules.

- [ ] **Stock Entities**:
    - Create `MouvementStock` entity for audit trails.
- [ ] **Stock Logic**:
    - Implement `StockService`.
    - **Automation**: Hook into the `Facture` creation process to automatically decrement stock based on invoice lines.
    - Implement logic for low-stock alerts (checking `SeuilAlerte`).
- [ ] **History**:
    - Ensure every stock change is logged in `MouvementStock`.

## Phase 4: Analytical Layer (Sprint 4)
**Goal**: Provide complex data views for the dashboard.

- [ ] **Advanced Queries**:
    - Implement LINQ queries for:
        - Total Turnover (CA) by day/month/year.
        - Total TVA collected.
        - Top-selling products.
        - Stock evolution over time.
- [ ] **DTO Optimization**:
    - Create specific Data Transfer Objects (DTOs) tailored for the dashboard to avoid overhead.

## Phase 5: Hardening & Finalization (Sprint 5)
**Goal**: Ensure reliability and performance.

- [ ] **Unit Testing**:
    - Write tests for the calculation logic (HT/TVA/TTC).
    - Test the stock decrement automation.
- [ ] **Optimization**:
    - Check for N+1 query issues.
    - Add database indexes on frequently queried columns (e.g., `DateFacture`, `ClientId`).
- [ ] **Documentation**:
    - Finalize clear API/Service documentation for the frontend team.
