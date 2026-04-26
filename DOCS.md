# API Documentation — Billing & Stock System

## Overview
The backend is built with .NET Web API and EF Core. It uses a **Result Pattern** for all services and a **Global Exception Middleware** for errors.

## Base Paths
- All controllers use standard routing (e.g., `/api/[controller]`).
- Swagger is available in Development mode via `/swagger` or `/openapi`.

## Core Services

### 🧾 Invoicing (FactureService)
- **Status Flow**: `Brouillon` -> `Validée` -> `Payée`.
- **Automation**: Stock is **automatically decremented** when an invoice is transitioned to `Validée`.
- **Calculations**: Handled server-side. Do not calculate totals on the frontend for financial records.

### 📦 Stock (StockService)
- **Alerte**: Consult the `GetProduitsEnAlerteAsync` endpoint for dashboard warnings.
- **Audit**: Every movement is logged in `MouvementsStock`.

## Global Tweaks
- **Soft Delete**: Entities have an `IsDeleted` property. Deleted records are **automatically filtered out** from all standard queries.
- **Error Handling**: Standardized JSON response:
  ```json
  {
    "statusCode": 500,
    "message": "Error details",
    "details": "Stack trace (Dev only)"
  }
  ```

## Security
- **Identity Framework**: Uses ASP.NET Core Identity.
- **Auth**: Protected routes require a valid token/session.
