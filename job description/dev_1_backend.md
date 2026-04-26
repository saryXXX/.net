# Dev 1 — Backend & Data Architect

## Responsabilité Centrale
Toute la logique métier.

## Missions
- **Modélisation base de données** : EF Core Code First.
- **Gestion des relations** : Client, Produit, Facture, Stock.
- **Implémentation des services** :
  - `FactureService`
  - `StockService`
  - `ProduitService`
- **Calculs** :
  - HT / TVA / TTC.
  - Décrément automatique du stock.
- **Migrations & DbContext**.
- **LINQ** : Requêtes complexes.
- **Sécurité** : Implémentation de ASP.NET Core Identity (Auth & rôles).
- **Architecture** : Gestion globale des erreurs et Soft Deletes (`IsDeleted`).

## Livrables
- Models propres.
- Services testés.
- Calculs fiables.
- Gestion stock automatique.
- Système d'authentification sécurisé.
- API résiliente (Error Handling standardisé).

## Organisation Agile (Sprints)

### Sprint 0 — Setup
- Créer DbContext.
- Config EF Core.
- **Setup Identity Framework**.
- **Middleware Global Error Handling**.

### Sprint 1 — Core CRUD
- Entités : Client, Produit.
- Implementation **Soft Deletes** (Global Query Filters).
- Migration DB.

### Sprint 2 — Facturation
- Facture + LigneFacture.
- Calculs (HT, TVA, TTC).

### Sprint 3 — Stock
- MouvementStock.
- StockService.
- Décrément auto.

### Sprint 4 — Dashboard
- Requêtes LINQ avancées.

### Sprint 5 — Finalisation
- Tests, Debug, UI polish (en collaboration).

## Planning Hebdomadaire (Exemple)
- **Jour 1** : Models
- **Jour 2** : Services
- **Jour 3** : Facturation
- **Jour 4** : Stock logic
