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

- `Web.API` owns controllers, HTTP concerns, middleware, authentication/authorization, security filters, and hosted jobs.
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

The API uses a **standardized error contract** based on **ProblemDetails (RFC 9457)**. All error responses (4xx and 5xx) must follow this format.

### Layer responsibilities

| Layer | Mechanism |
| :--- | :--- |
| **Application** | Return `ErrorOr<T>` for expected business failures. Never return HTTP results. |
| **Web.API (Controllers)** | Translate `ErrorOr<T>` to `IActionResult` via `ErrorOrAspNetCoreExtensions`. |
| **Web.API (Filters/Middleware)** | Use `ApiProblemDetailsFactory` to build `ProblemDetails` for cross-cutting concerns. |
| **Global Exceptions** | Handled by `GlobalExceptionHandler` (`IExceptionHandler`). |

### Application layer: `ErrorOr<T>`

Expected application/domain failures must use `ErrorOr<T>`. Application code must never return `Ok()`, `BadRequest()`, `NotFound()`, `Unauthorized()`, or other HTTP results.

```csharp
Task<ErrorOr<Guid>> DeleteCertificateAsync(Guid id);
```

### API layer: `ErrorOrAspNetCoreExtensions`

Controllers translate `ErrorOr<T>` results through extension methods. Avoid `Match(..., errors => Problem(errors))` manual mapping unless the extension does not fit.

```csharp
[HttpDelete("{id}")]
[Authorize(Policy = "AdminOnly")]
public async Task<IActionResult> Delete(Guid id)
{
    var result = await _service.DeleteCertificateAsync(id);
    return result.ToOkWithoutBody();
}
```

Default mapping (from `ErrorOrAspNetCoreExtensions`):

| `ErrorType` | HTTP Status |
| :--- | :--- |
| `Validation` | 400 Bad Request |
| `Unauthorized` | 401 Unauthorized |
| `Forbidden` | 403 Forbidden |
| `NotFound` | 404 Not Found |
| `Conflict` | 409 Conflict |
| `Unexpected` / others | 500 Internal Server Error |

### Cross-cutting concerns: `ApiProblemDetailsFactory`

For filters, middleware, or any location that does not consume `ErrorOr<T>`, use `ApiProblemDetailsFactory` to construct consistent `ProblemDetails` responses.

```csharp
context.Result = new ObjectResult(
    ApiProblemDetailsFactory.TooManyRequests(entry.BlockedUntil.Value))
{
    StatusCode = StatusCodes.Status429TooManyRequests
};
```

Do not construct `ProblemDetails` manually. The factory ensures:

- Consistent `type` URIs (`https://tools.ietf.org/html/rfc9110#section-...`).
- Consistent `title`, `detail`, and extension fields.
- Easy evolution of the error contract.

### Global exception handling: `IExceptionHandler`

Unexpected exceptions are caught by `GlobalExceptionHandler`, which:

- Logs the exception with Serilog (`traceId`, method, path).
- Reports the incident to Observability (`IErrorReporter`, fire-and-forget).
- Returns a 500 `ProblemDetails` response with `traceId`.

Do not use `try-catch` for expected business failures in the Application layer. Only catch exceptions when there is a clear handling strategy (retry, fallback).

Do not wrap MediatR calls or handler logic in `try-catch` blocks in the service layer to convert exceptions to `ErrorOr`. Let unexpected exceptions propagate to the global handler.

### `ProblemDetails` configuration

`AddProblemDetails()` is registered in `AddPresentation()`. `UseExceptionHandler()` is registered in `Program.cs` before `UseRouting()`. Do not duplicate `traceId` — .NET already adds it by default.

## Authentication and Authorization

The API is protected by default.

- Assume new endpoints require authentication unless explicitly public.
- Use `[AllowAnonymous]` only intentionally.
- Reuse existing authorization policies, including `AdminOnly` and `UserOnly` where applicable.
- Do not duplicate JWT validation logic.
- Do not implement inconsistent role checks inside controllers.

## Security: Rate Limiting & CAPTCHA

Protected endpoints (`login`, `register`, `forgot-password`, `reset-password`) are guarded by `SecurityGuardFilter`, which enforces IP-based rate limiting and CAPTCHA (Cloudflare Turnstile).

### Architecture

| Component | Responsibility | Location |
| :--- | :--- | :--- |
| `SecurityGuardFilter` | Orchestrates the flow | `Web.API/Filters` |
| `ClientIpResolver` | Extracts client IP (respects `CF-Connecting-IP`) | `Web.API/Security` |
| `ProtectedActionResolver` | Normalizes action name and checks if protected | `Web.API/Security` |
| `CaptchaRequiredPolicy` | Decides when CAPTCHA is required | `Web.API/Security` |
| `IIpAttemptTrackingService` | Tracks failures and blocking per IP/action | `Application` + `Infrastructure` |
| `ITurnstileValidator` | Validates Turnstile tokens against Cloudflare | `Application` + `Infrastructure` |

### Failure tracking rules

- **Counter** is per `{IP}_{action}` (e.g., `throttle_1.2.3.4_login`).
- **Increment** on: 401 (invalid credentials), 400 (invalid CAPTCHA), 428 (CAPTCHA required, no token).
- **Do NOT increment** on: 200-2xx (success), 429 (already blocked).
- **Reset** only on: 200 (successful authentication).
- **CAPTCHA valid does NOT reset the counter.** Only a successful login resets it.

### Thresholds (configurable in `appsettings.json`)

```json
"Security": {
  "RateLimiting": {
    "MaxAttempts": 6,
    "TimeWindowMinutes": 15,
    "BlockDurationMinutes": 15,
    "CaptchaRequiredAttempts": 3
  }
}
```

- **`CaptchaRequiredAttempts`**: after N failures, CAPTCHA is required.
- **`MaxAttempts`**: after N failures, IP is blocked (429) for `BlockDurationMinutes`.
- **`TimeWindowMinutes`**: window for counting failures (sliding expiration).

### Response contract

| Status | When | ProblemDetails extensions |
| :--- | :--- | :--- |
| **428 Precondition Required** | CAPTCHA required, no token provided | `requiresCaptcha: true` |
| **429 Too Many Requests** | IP blocked | `blockedUntil` (ISO 8601) + `Retry-After` header |
| **400 Bad Request** | Invalid CAPTCHA token | (standard ProblemDetails) |
| **503 Service Unavailable** | Turnstile service unreachable | (standard ProblemDetails) |

### Blocking behavior

The counter is checked **immediately after incrementing**. If the increment triggers the block, return 429 in the same response — do not wait for the next request. This avoids the "misleading 428" UX issue where the user thinks they have another chance but is already blocked.

### Integration with frontend

- The frontend renders the Turnstile widget on `428` and submits the `captchaToken` in the request body.
- On `429`, the frontend must enter a blocked state with a countdown (using `blockedUntil`), disable inputs, and hide the CAPTCHA widget.
- The Turnstile widget uses `data-permanent` and exposes a `reset()` method via `forwardRef` + `useImperativeHandle` to avoid re-mounting on every render.
- Tokens are single-use. After a failed attempt, reset the widget to obtain a fresh token.
- Reference implementation: `frontend/src/Security/Hooks/useLoginFlow.ts` and `frontend/src/Security/Components/TurnstileWidget.tsx`.

### IP resolution

Use `CF-Connecting-IP` header first (Cloudflare proxied traffic), fall back to `RemoteIpAddress`. Implemented in `ClientIpResolver`.

### Known pending work

- Separate request counter per IP to block "infinite 428" attacks (DoS to the CAPTCHA service). To be implemented as a complement to the failure counter.

## Persistence

- Repository abstractions belong in Domain.
- Repository implementations and EF configuration belong in Infrastructure.
- Use `ApplicationDbContext` and `IUnitOfWork`.
- Preserve existing schemas, relationships, conversions, indexes, constraints, nullability, and delete behavior.
- Generate EF migrations for database changes.
- Do not silently change database behavior.
- Follow active read-only query patterns such as `AsNoTracking` where appropriate.
- When performing multiple operations (e.g., saving a token and sending an email), persist critical data first, then perform side effects (notifications), and finally commit all changes in a single transaction.
- Use `IUnitOfWork` to coordinate multiple repository changes in one `SaveChangesAsync`.

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

## Observability Integration

The API reports unhandled exceptions to an external Observability Platform (ObsPlatform) via HTTP.

### Architecture

```text
GlobalExceptionHandler
-> IErrorReporter.ReportAsync (fire-and-forget)
-> ObservabilityErrorReporter
-> HTTP POST /api/v1/{appId}/incidents (X-API-Key)
-> ObsPlatform
```

### Configuration

The `Observability` section in `appsettings.{Environment}.json`:

```json
"Observability": {
  "BaseUrl": "https://obs.csingenieria.com.ar",
  "ApiId": "{appId}",
  "ApiKey": "obs_live_..."
}
```

The API key is generated automatically by the CI/CD pipeline on each deploy (see the `Deploy to Staging` workflow).

### Rules

- Reports are **fire-and-forget**. Never block the HTTP response on the reporting call.
- If configuration is missing, log a warning and skip (do not throw).
- If the report endpoint returns 404 (not implemented), log at debug level and skip.
- If the request fails (network error), log the failure but do not propagate.
- The payload includes `message`, `exceptionType`, `source`, `stackTrace`, `level`, `environment`, and `context` (method, path, traceId, user).

### Do NOT

- Do not throw from `IErrorReporter`.
- Do not block on the report call.
- Do not include sensitive data (passwords, tokens) in the payload.

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
- Controllers still use `Match(..., errors => Problem(errors))` instead of `ErrorOrAspNetCoreExtensions`.
- Incomplete Customer repositories.
- Partial pagination.
- Domain-event lifecycle requiring verification.
- Naming inconsistencies.
- Mixed AutoMapper/manual mapping.
- Inconsistent CancellationToken propagation.
- Mixed older/newer application patterns.
- No request-level counter to mitigate "infinite 428" attacks.
- No `/releases` endpoint in ObsPlatform.

Do not fix these automatically during unrelated work. Address them only when explicitly requested or directly required by the feature.

## Completion Checklist

- [ ] Correct active module identified.
- [ ] Active implementation used as reference.
- [ ] No Sales/Customer/RabbitMQ legacy pattern copied.
- [ ] Architectural boundaries preserved.
- [ ] FluentValidation added when appropriate.
- [ ] `ErrorOr<T>` used for expected failures.
- [ ] Controllers use `ErrorOrAspNetCoreExtensions` where applicable.
- [ ] Cross-cutting errors use `ApiProblemDetailsFactory`.
- [ ] No manual `ProblemDetails` construction outside the factory.
- [ ] Protected endpoints (`login`, `register`, `forgot-password`, `reset-password`) keep the `SecurityGuardFilter` behavior.
- [ ] Rate limiting counter is not reset by valid CAPTCHA (only by successful login).
- [ ] Global exceptions are handled by `GlobalExceptionHandler` (not by custom middleware).
- [ ] Observability reports are fire-and-forget.
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

## Event Handling & Token Lifecycle

When handling domain events that create tokens for password reset, account activation, or similar actions:

- **Create and persist** the token first, before sending any external notification (email, SMS).
- **Invalidate any previous active tokens** for the same user and purpose immediately after persisting the new token.
- **Send the notification** (email) after the token is safely stored; if the notification fails, mark the notification as failed but keep the token (it can be retried or the user can request another).
- **Save changes** (`IUnitOfWork.SaveChangesAsync`) once at the end, after all modifications are completed.

Do not rely solely on token invalidation at the time of token usage (e.g., password reset) to handle multiple requests; invalidate proactively on new token creation.