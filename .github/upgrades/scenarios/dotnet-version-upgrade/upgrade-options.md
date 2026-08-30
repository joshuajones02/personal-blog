# Upgrade Options

## Upgrade Strategy

How the upgrade is ordered and executed across projects.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all 7 projects together in a single atomic pass, then validate the full solution. Best fit: small solution, all projects already on modern .NET 8, SDK-style, shallow dependency graph. |
| Bottom-Up | Upgrade leaf libraries first, validate each tier before the next. Adds overhead not warranted for a solution of this size. |
| Top-Down | Upgrade the app first with multi-targeted libraries. Unnecessary complexity here. |
