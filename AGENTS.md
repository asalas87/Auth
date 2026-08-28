# Repository AI Guidance

## Project scope

The active backend is the main ASP.NET Core API under `backend/`. Treat these as authoritative active areas:

- Security
- Partners
- Documents
- Controls
- Active Notifications functionality

Do not use Sales, Customer, `Notifications.Worker`, or the RabbitMQ notification implementation as architectural references. They are legacy, unused, experimental, or out of scope. Legacy/unused code must not be used as a pattern source.

## Before coding

1. Identify the affected bounded context/module.
2. Search active modules for similar implementations.
3. Read `.kilocode/skills/backend-development/SKILL.md`.
4. Determine which application layers are affected.
5. Check existing abstractions before introducing new ones.
6. Create a concise implementation plan before substantial changes.

## During coding

- Preserve the existing architecture and keep controllers thin.
- Put business logic in the appropriate layer and follow active patterns.
- Avoid unrelated refactoring and new libraries unless necessary.
- Do not copy legacy patterns.
- Propagate `CancellationToken` through new asynchronous layers where appropriate.
- Use FluentValidation for new request validation when appropriate.
- Return `ErrorOr<T>` for expected application/domain failures.
- Use centralized `Problem(errors)` mapping in the API.
- Use the existing SQL-backed `NotificationsJob` architecture; do not introduce RabbitMQ.

## After coding

- Review the final diff and architectural boundaries.
- Build the affected project.
- Run relevant tests.
- Check migrations when persistence changes.
- Verify authorization requirements.
- Confirm no legacy patterns were introduced.
