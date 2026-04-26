# Sprint 0: Analytics Setup

## 1. Graphics Library Selection
**Choice: [ApexCharts](https://apexcharts.com/)**
- **Rationale**: 
  - Rich interactive features (zoom, pan, tooltips).
  - Native-like integration with Blazor via `Blazor-ApexCharts`.
  - Responsive by default.
  - Supports all required chart types (Area, Bar, Donut).

## 2. Defined KPIs (Key Performance Indicators)

### A. Fiscal Analytics
- **Total Revenue (CA)**: Gross sales amount excluding taxes.
- **VAT Collected (TVA)**: Total tax amount collected per period.
- **Monthly Growth**: Percentage change in revenue month-over-month.

### B. Sales Analytics
- **Top 5 Products**: Products generating the highest revenue.
- **Sales Volume**: Number of transactions per day/week.
- **Customer Breakdown**: Sales distribution across different segments (if applicable).

### C. Stock & Inventory Analytics
- **Out-of-Stock Items**: Count of products with zero inventory.
- **Low Stock Alerts**: Items below the minimum threshold.
- **Inventory Turnover**: Speed at which stock is sold and replaced.

## 3. Technical Requirements
- **Backend**: Advanced LINQ queries in `AnalyticsController`.
- **Frontend**: Dynamic dashboard layout in the `Dashboard` project.
- **Data Resiliency**: Default values (0) for periods with no data to prevent chart breaking.
