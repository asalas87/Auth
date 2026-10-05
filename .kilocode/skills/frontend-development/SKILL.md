Frontend Development Skill
Scope and Source of Truth
The React frontend under frontend/ is the source of truth. The backend API contract lives under backend/. Do not invent endpoints or DTOs — verify against backend controllers.

Active modules:

Security (auth flows, users)

Documents (documents, certificates, renovations, WPS/PQR)

Partners (companies, lookups)

Architecture

src/
├── app/                    app shell, providers, routing
├── components/             shared presentational components (Bootstrap)
├── features/               domain modules (feature-based)
│   ├── documents/
│   ├── partners/
│   └── security/
├── hooks/                  cross-feature hooks
├── lib/                    API client, auth helpers, utilities
├── settings/               runtime configuration
└── types/                  global type declarations
Boundaries

features/<name>/ owns everything domain-specific.

components/ owns generic, feature-agnostic UI.

hooks/ owns cross-feature hooks (used by 2+ features).

lib/ owns infrastructure utilities.

app/ owns composition.

Feature Module Structure
Every feature follows the Partners shape (folders in PascalCase):

features/<name>/
├── Routes.tsx
├── Interfaces/
│   └── Dtos/               IXxxDTO, IXxxEditDTO, IXxxResponseDTO
├── Services/               *Service.ts
├── Context/                optional
├── Hooks/                  optional
└── Views/
    ├── XxxView.tsx
    └── Forms/
        ├── XxxEditForm.tsx
        └── XxxColumns.tsx
Naming Conventions
Element	Convention	Example
Folders	PascalCase inside features; lowercase at src/ root	Views/, Services/, components/, lib/
Components	PascalCase.tsx	LoginPanel.tsx, CertificateEditForm.tsx
Hooks	camelCase.ts with use prefix	useLoginFlow.ts
Services	camelCase.ts with Service suffix	certificateService.ts
DTOs	PascalCase.ts with I prefix	ICertificateDTO.ts
Helpers (in lib/)	camelCase.ts	api.ts, auth.ts, dates.ts, errorHandling.ts, loadScript.ts
Routes	Routes.tsx	features/documents/Routes.tsx
Language: identifiers in English, UI strings in Spanish.

Anti-patterns: mixed casing in one folder, typos (reonvationColumns), RegistrosDeCalificacion*, Tailwind classes.

DTOs
Each entity has three DTOs in Interfaces/Dtos/:

IXxxDTO — Create (POST)

IXxxEditDTO — Update (PUT)

IXxxResponseDTO — Response (GET)

Shared types (PagedResult<T>) live in src/types/. Never duplicate.

State Management
Context — global shared state. Use only for auth, loading, theme.

Rules:

Typed with undefined default.

Consumed through a throwing hook (useAuthContext throws if outside provider).

Never default to {} or no-op functions.

Hooks — reusable stateful logic. Extract on the 3rd repetition.

Shared hooks (in src/hooks/):

usePaginatedList — paginated data fetching for CRUD views.

Reference: features/security/Context/AuthContext.ts + AuthProvider.tsx + useAuthContext.

Services and API Layer
All services use the shared api axios instance from lib/api.ts. No raw axios.

No throw new Error for validation — validation belongs in forms.

Import paths must use the @/ alias.

No circular imports between lib/api.ts and feature services.

Forms
Use GenericEditForm from components/EditForm/ with a declarative FieldConfig<T>[].

Field patterns:

MUI TextField wrappers (EmailField, PasswordField) — auth screens.

GenericEditForm + FieldConfig — CRUD screens.

Not allowed: raw <input className="form-control">.

Routing
Each feature exports a Routes.tsx (default export: RouteObject[]).

app/routes.tsx composes them.

app/ProtectedRoute.tsx guards protected routes.

No hardcoded paths in components.

All routes have a catch-all 404.

Lazy load feature routes.

Styling
Single system: Bootstrap.

Classes from the bootstrap npm package (not CDN).

Inline styles only for dynamic values.

MUI allowed ONLY for: DataGrid, TextField, Dialog, IconButton, Snackbar.

No Tailwind. No CSS modules. No styled-components.

Common Components
Located under src/components/, all PascalCase folders:

Component	Purpose
DataGrid (MUI)	Paginated tables with actions
EditForm	Generic declarative form engine
Fields	MUI-based auth inputs (EmailField, PasswordField)
Button	Single Bootstrap button
Heading	Single heading
PageHeader, RowActions	Shared layout primitives
Layout, Header, Sidebar, TopBar, ProgressBar	App shell
ErrorBoundary	React error boundary
CsIngenieriaLogo	Brand logo
One component per concept. No duplicates.

Do NOT
Do not create files under atoms/ or molecules/.

Do not use Tailwind classes.

Do not import @mui/material outside the allowed list.

Do not create a PagedResult<T> — use the shared one from types/.

Do not duplicate useBlockCountdown logic — import the hook.

Do not use RegistroDeCalificacion* naming. Use Certificate*.

Do not add try-catch for API errors — the interceptor handles them.

Do not use raw axios — use api from lib/api.ts.

Do not use relative import paths — use @/.

Completion Checklist
□ Feature follows the Partners shape (PascalCase folders).
□ Services use the shared api instance.
□ DTOs split into DTO / EditDTO / ResponseDTO.
□ Forms use GenericEditForm (or MUI auth pattern).
□ Route file exports RouteObject[].
□ No hardcoded paths in components.
□ No Tailwind, no atoms/, no molecules/.
□ No duplicated PagedResult<T>.
□ No circular imports.
□ npm run build passes.
□ npm run lint passes with 0 warnings.
Known Technical Debt
expirationDate vs validity — two names for the same concept in IDocumentDTO, ICertificateDTO, IRenovationDTO. Reconcile with the backend in a future iteration.

PagedResult<T> — declared once in types/, but legacy services may still declare their own. Migrate when touched.

Migration Reference
See FRONTEND-ANALYSIS.md at the repo root for the full analysis that drove the refactor.