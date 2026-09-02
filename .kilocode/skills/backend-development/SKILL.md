# Backend Development Skill

## Scope and Source of Truth

The active main API under `backend/` is the source of truth for current backend development. Prioritize active modules:

- Security
- Partners
- Documents
- Controls
- Active Notifications functionality

Do not use Sales, Customer, `Notifications.Worker`, or the RabbitMQ notification implementation as reference implementations. They are legacy, unused, experimental, or out of scope. When searching for examples, prioritize active modules and repeated patterns in active production-used code.

Clearly distinguish current behavior from target direction. Do not infer standards from unused or abandoned code.

## Architecture

The active request flow is:

```text
Web.API
-> Application service
-> MediatR command/query
-> ValidationBehavior
-> Handler
-> Domain
-> Repository abstraction
-> Infrastructure repository
-> ApplicationDbContext
-> SQL Server
```

Responsibilities and boundaries:

- `Web.API` owns controllers, HTTP concerns, middleware, authentication/authorization, and hosted jobs.
- `Application` owns use cases, application services, commands, queries, handlers, validators, DTOs, and application abstractions.
- `Domain` owns entities, aggregate roots, value objects, domain events, and repository abstractions.
- `Infrastructure` owns EF Core, repository implementations, EF configurations, migrations, and external integrations.
- `SharedKernel` contains shared contracts/entities used by the active backend.

Preserve these rules:

- Domain must not depend on Infrastructure or Web.API.
- Repository interfaces belong in Domain.
- Repository implementations and EF configuration belong in Infrastructure.
- Controllers remain thin and must not contain business logic.
- Application code must not construct HTTP responses.
- Access Infrastructure integrations through Application abstractions when appropriate.
- Use `IUnitOfWork` and `ApplicationDbContext` for persistence commits.

## Feature Implementation Workflow

### Step 1 - Understand

Understand the requirement and identify the affected bounded context.

### Step 2 - Find references

Search active modules for the closest existing implementation. Use Company and Document features as primary references. Never use Sales, Customer, or `Notifications.Worker` as implementation references.

### Step 3 - Determine affected layers

Evaluate Domain, Application, Infrastructure, API, database, and tests. Modify only necessary layers.

### Step 4 - Plan

Before substantial implementation, state a short plan naming the affected files/layers and the focused validation to run.

### Step 5 - Implement

Follow the active local pattern. Prefer extending existing abstractions over introducing new ones. Avoid unrelated refactoring.

### Step 6 - Validate

Build and test the affected functionality. Add a migration when persistence changes require one.

### Step 7 - Review

Review the diff, dependency direction, authorization, error contract, and legacy-pattern exclusions.

## Create / Update / Delete Patterns

Use these active implementations as references:

- `backend/Application/Partners/Companies/Create/CreateCompanyCommandHandler.cs`
- `backend/Application/Partners/Companies/Update/UpdateCompanyCommandHandler.cs`
- `backend/Application/Partners/Services/CompanyService.cs`
- `backend/Application/Documents/Services/DocumentService.cs`
- `backend/Infrastructure/Persistence/Partners/Repositories/CompanyRepository.cs`
- `backend/Web.API/Controllers/Partners/CompanyController.cs`

The usual pattern is controller -> application service -> MediatR request -> handler -> repository/domain -> `IUnitOfWork`. Do not copy legacy Customer/Sales implementations.

## Validation

### Current state

FluentValidation is integrated through MediatR `ValidationBehavior`, but validator coverage is incomplete. Treat missing validators in older code as technical debt.

### Target state

New commands/requests should use FluentValidation whenever request validation is appropriate.

- **Request validation** (null checks, format, length, required fields, etc.) must be implemented using FluentValidation validators associated with the command/query.
- **Business validation** (existence, state, permissions, token validity, etc.) that requires repository access or domain logic belongs in the handler.
- **Handlers must not contain request-level validations** if a FluentValidation validator exists; avoid duplication.
- **Application service layer** (if used) must not contain any validation logic; it only orchestrates the use case.
- Do not duplicate domain invariants in validators; keep domain rules in entities/value objects.
- For Value Object comparisons (e.g., `UserId`), use `.Value` or `Equals` to compare underlying values; avoid direct `==` unless overloaded.

## Error Handling

The target pattern is:

Application layer:

```csharp
ErrorOr<T>
```

API layer:

```csharp
return result.Match(
    value => Ok(value),
    errors => Problem(errors)
);
```

Expected application/domain failures must use `ErrorOr<T>`. Application code must never return `Ok()`, `BadRequest()`, `NotFound()`, `Unauthorized()`, or other HTTP results.

Controllers translate errors through centralized `Problem(errors)` mapping. Do not introduce `BadRequest(result.FirstError)` unless there is a clear, explicitly justified reason. Expected business failures should not use exceptions. Unexpected exceptions continue through the global exception middleware.

## Authentication and Authorization

The API is protected by default.

- Assume new endpoints require authentication unless explicitly public.
- Use `[AllowAnonymous]` only intentionally.
- Reuse existing authorization policies, including `AdminOnly` and `UserOnly` where applicable.
- Do not duplicate JWT validation logic.
- Do not implement inconsistent role checks inside controllers.

## Persistence

- Repository abstractions belong in Domain.
- Repository implementations and EF configuration belong in Infrastructure.
- Use `ApplicationDbContext` and `IUnitOfWork`.
- Preserve existing schemas, relationships, conversions, indexes, constraints, nullability, and delete behavior.
- Generate EF migrations for database changes.
- Do not silently change database behavior.
- Follow active read-only query patterns such as `AsNoTracking` where appropriate.

## Notifications

The active architecture is:

```text
Main API
-> NotificationsJob
-> notification application service
-> SQL/database persistence
-> email processing/sending
```

The daily hosted `NotificationsJob` is the current notification mechanism. Do not introduce RabbitMQ or use the separate `Notifications.Worker` for new functionality.

`UserCreatedEventHandler` is currently relevant and handles the user-creation notification email. Before modifying it, trace the entity/domain-event creation, event publication, handler, persistence, and email flow. Do not assume the domain-event lifecycle is complete without verifying it in code.

## Pagination

Pagination is partially prepared but is not fully integrated with the React grids. Some methods accept Page, PageSize, and Filter while retrieval may not yet apply `Skip` and `Take`.

- Do not treat the partial implementation as the final pagination pattern.
- Preserve existing parameters unless removal is explicitly required.
- Do not silently change pagination semantics during unrelated work.
- Implement pagination together with the frontend API contract when requested.
- Prefer a consistent response containing items and total count.

## CancellationToken

Cancellation propagation is inconsistent in existing code. New asynchronous code should propagate `CancellationToken` through API/request, Application, handler, repository, and EF Core layers where appropriate. Use cancellation-aware EF Core APIs. Do not perform unrelated refactoring solely to normalize old code.

## Mapping, Constructors, and Naming

The project uses both AutoMapper and manual mapping, and both primary and traditional constructors. Naming also varies between DTO, Dto, and Request. Do not introduce a new mapping approach or broad naming normalization. Follow the active local module convention when adding code.

## Code Comments

Production code should be self-explanatory through meaningful names and clear structure.

- Avoid unnecessary comments that merely describe what the code obviously does.
- Do not add explanatory comments for standard operations (validation, error handling, object creation, etc.).
- Do not comment obvious intent: `// Validate token`, `// Check user`, `// Hash password`, `// Return error`.
- Keep comments concise. Do not generate large blocks of explanatory text.
- Comments should only be used when they provide genuinely useful context that cannot be expressed clearly through code itself.
- Prefer expressive naming, clear structure, and domain language over explanatory comments.
- Valid reasons for comments:
  - Non-obvious business or domain rules.
  - Architectural decisions or trade-offs.
  - Security or performance considerations.
  - References to external specifications or requirements.
  - Workarounds or gotchas that are not obvious from the code.
- When modifying existing code, remove unnecessary comments in the touched code where appropriate.

## Testing

When changing backend behavior:

- Add or update focused tests for handlers, validators, and important domain behavior where supported.
- Build the affected project or solution.
- Run relevant tests.
- Validate migration generation/build when persistence changes.
- Do not create unrelated large test suites.

## Legacy Protection Rules

Never use these as implementation references:

```text
Sales
Customer
Notifications.Worker
RabbitMQ notification implementation
```

If an active and legacy implementation both exist, always prefer the active one. If only a legacy implementation exists:

1. Analyze the intended architecture.
2. Check newer active modules.
3. Identify which parts remain valid.
4. Explain any necessary deviation before implementing.

## Technical Debt Awareness

Known technical debt includes:

- Incomplete FluentValidation coverage.
- Inconsistent error mapping in older endpoints.
- Incomplete Customer repositories.
- Partial pagination.
- Domain-event lifecycle requiring verification.
- Naming inconsistencies.
- Mixed AutoMapper/manual mapping.
- Inconsistent CancellationToken propagation.
- Mixed older/newer application patterns.

Do not fix these automatically during unrelated work. Address them only when explicitly requested or directly required by the feature.

## Completion Checklist

- [ ] Correct active module identified.
- [ ] Active implementation used as reference.
- [ ] No Sales/Customer/RabbitMQ legacy pattern copied.
- [ ] Architectural boundaries preserved.
- [ ] FluentValidation added when appropriate.
- [ ] `ErrorOr<T>` used for expected failures.
- [ ] Centralized `Problem(errors)` mapping used.
- [ ] `CancellationToken` propagated in new async code.
- [ ] Authorization requirements verified.
- [ ] Database changes include an appropriate migration.
- [ ] Relevant tests added or updated.
- [ ] Build succeeds.
- [ ] Final diff contains no unrelated changes.

## Important AI Behavior

When uncertain between patterns:

1. Prefer the newest active production-used implementation.
2. Search for additional active examples.
3. Do not invent a new pattern merely because legacy code is inconsistent.
4. State uncertainty before making a significant architectural decision.

The goal is safe, consistent evolution of the existing system, not architectural reinvention.
