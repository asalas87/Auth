# Frontend Development Skill

## Scope and Source of Truth

The React frontend under `frontend/` is the source of truth. The backend API contract lives under `backend/`. Do not invent endpoints or DTOs — verify against backend controllers.

Active modules:
- Security (auth flows, users)
- Documents (documents, certificates, renovations, WPS/PQR)
- Partners (companies, lookups)

Legacy / to be removed:
- `atoms/` and `molecules/` (Atomic Design remnants)
- Tailwind classes (never installed)
- `RegistrosDeCalificacion*` naming (superseded by `Certificate*`)
- `Controls/` (consolidating into `Partners`)
- `CompanySelect` (unused)

## Architecture
src/
├── app/ app shell, providers, routing
├── components/ shared presentational components (Bootstrap)
├── features/ domain modules (feature-based)
│ ├── security/
│ ├── documents/
│ └── partners/
├── hooks/ cross-feature hooks
├── lib/ API client, auth helpers, utilities
├── types/ global type declarations
└── settings/ runtime configuration


**Boundaries**

- `features/<name>/` owns everything domain-specific.
- `components/` owns generic, feature-agnostic UI.
- `hooks/` owns cross-feature hooks (used by 2+ features).
- `lib/` owns infrastructure utilities.
- `app/` owns composition.

## Feature Module Structure

Every feature follows the **Partners shape**:
features/<name>/
├── routes.tsx
├── interfaces/
│ └── dtos/ IXxxDTO, IXxxEditDTO, IXxxResponseDTO
├── services/ *Service.ts
├── hooks/ optional
├── context/ optional
└── views/
├── XxxView.tsx
└── forms/
├── XxxEditForm.tsx
└── xxxColumns.tsx


## Naming Conventions

| Element | Convention | Example |
|---|---|---|
| Folders | `kebab-case` or `lowercase` | `common/`, `components/`, `hooks/` |
| Components | `PascalCase.tsx` | `LoginPanel.tsx` |
| Hooks | `camelCase.ts` with `use` prefix | `useLoginFlow.ts` |
| Services | `camelCase.ts` with `Service` suffix | `certificateService.ts` |
| DTOs | `PascalCase.ts` with `I` prefix | `ICertificateDTO.ts` |
| Helpers | `camelCase.ts` | `api.ts`, `authHelpers.ts` |
| Routes | `routes.tsx` | `features/documents/routes.tsx` |

**Language**: identifiers in **English**, UI strings in **Spanish**.

**Anti-patterns**: mixed casing in one folder, typos (`reonvationColumns`), `RegistrosDeCalificacion*`.

## DTOs

Each entity has three DTOs in `interfaces/dtos/`:
- `IXxxDTO` — Create (POST)
- `IXxxEditDTO` — Update (PUT)
- `IXxxResponseDTO` — Response (GET)

Shared types (`PagedResult<T>`) live in `src/types/`. Never duplicate.

## State Management

**Context** — global shared state. Use only for auth, loading, theme.

**Rules**:
- Typed with `undefined` default.
- Consumed through a throwing hook (`useAuthContext` throws if outside provider).
- Never default to `{}` or no-op functions.

**Hooks** — reusable stateful logic. Extract on the 3rd repetition.

**Reference**: `Security/Context/AuthContext.ts` + `AuthProvider.tsx` + `useAuthContext`.

## Services and API Layer

- All services use the shared `api` axios instance from `lib/api.ts`. No raw `axios`.
- No `throw new Error` for validation — validation belongs in forms.
- Import paths must use the `@/` alias.
- No circular imports between `lib/api.ts` and feature services.

## Forms

Use `GenericEditForm` from `components/EditForm/` with a declarative `FieldConfig<T>[]`.

**Field patterns**:
- MUI `TextField` wrappers (`EmailField`, `PasswordField`) — auth screens.
- `GenericEditForm` + `FieldConfig` — CRUD screens.

**Not allowed**: raw `<input className="form-control">`.

## Routing

- Each feature exports a `routes.tsx` (default export: `RouteObject[]`).
- `app/routes.tsx` composes them.
- `app/ProtectedRoute.tsx` guards protected routes.
- No hardcoded paths in components.
- All routes have a catch-all 404.
- Lazy load feature routes.

## Styling

**Single system: Bootstrap.**

- Classes from `bootstrap` package (npm, not CDN).
- Inline styles only for dynamic values.
- MUI allowed ONLY for: `DataGrid`, `TextField`, `Dialog`, `IconButton`, `Snackbar`.
- No Tailwind. No CSS modules. No styled-components.

## Common Components

| Component | Purpose |
|---|---|
| `DataGrid` (MUI) | Paginated tables with actions |
| `EditForm` | Generic declarative form engine |
| `EmailField`, `PasswordField` | MUI-based auth inputs |
| `Layout`, `Header`, `Sidebar`, `TopBar`, `ProgressBar` | App shell |
| `ErrorBoundary` | React error boundary |
| `Button` | Single Bootstrap button |
| `Heading` | Single heading |

**One component per concept.** No duplicates.

## Do NOT

- Do not create files under `atoms/` or `molecules/` (being deleted).
- Do not use Tailwind classes.
- Do not import `@mui/material` outside the allowed list.
- Do not create a `PagedResult<T>` — use the shared one.
- Do not duplicate `useBlockCountdown` logic — import the hook.
- Do not use `RegistroDeCalificacion*` naming. Use `Certificate*`.
- Do not add `try-catch` for API errors — the interceptor handles them.
- Do not use raw `axios` — use `api`.
- Do not use relative import paths — use `@/`.

## Completion Checklist

- [ ] Feature follows `Partners` shape.
- [ ] Services use the shared `api` instance.
- [ ] DTOs split into `DTO` / `EditDTO` / `ResponseDTO`.
- [ ] Forms use `GenericEditForm` (or MUI auth pattern).
- [ ] Route file exports `RouteObject[]`.
- [ ] No hardcoded paths in components.
- [ ] No Tailwind, no `atoms/`, no `molecules/`.
- [ ] No duplicated `PagedResult<T>`.
- [ ] No circular imports.
- [ ] `npm run build` passes.
- [ ] `npm run lint` passes.

## Pending Decisions

- **`expirationDate` vs `validity`** — reconcile with backend in a future iteration.
- **`isAuthenticated` / `isAdmin`** — removed from `AuthContext` (only `user` + `user.role`).
- **`/activate`** — stays public (no `ProtectedRoute`).

## Migration Sequence (reference)

See `FRONTEND-ANALYSIS.md` §13 and §14 for the full migration plan. Phases:

1. Quick wins (delete orphans, fix phantom deps, add eslint).
2. Fix Tailwind breakage.
3. Rename sweep.
4. Consolidate `Common/`.
5. Move `Controls` into `Partners`.
6. Restructure to `features/`.
7. Extract shared hooks.

Each phase = one commit. No mixing rename with behavioral changes.