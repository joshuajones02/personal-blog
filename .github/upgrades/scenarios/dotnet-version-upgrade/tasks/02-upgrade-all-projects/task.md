# 02-upgrade-all-projects: Upgrade all projects to net10.0 and update packages

Update TargetFramework from net8.0 to net10.0 across all 7 projects (Home.Blog.Common, Home.Blog.Core, Home.Blog.Data, Home.Blog.Mvc, and the three test projects). Bump recommended packages: Microsoft.Extensions.Caching.Memory, Microsoft.Extensions.Configuration, System.Formats.Asn1, System.Text.Json → 10.0.x, and align other Microsoft.Extensions.*/EF Core/ASP.NET Core packages (currently 8.0.x) to 10.0.x. Address flagged packages: Microsoft.VisualStudio.Azure.Containers.Tools.Targets (incompatible — update or remove), deprecated Azure.Identity 1.11.4 (update to current), System.IdentityModel.Tokens.Jwt (transitive/deprecated — verify), and deprecated xunit 2.x (update to current supported version). Assessment flagged one behavioral-change API issue in Home.Blog.Common — investigate and fix during the pass. Restore, build the full solution, and fix all compilation errors and warnings in a single bounded pass.

Research starting points: check Directory.Build.props for centralized TFM; verify Piranha CMS 12.x compatibility with net10.0 before bumping ASP.NET Core packages; review the Api.0003 behavioral change detail in assessment.md.

**Done when**: All 7 projects target net10.0; recommended package updates applied; solution builds with 0 errors and 0 warnings in modified projects.

## Research Findings

**TFM changes**:
- Root `Directory.Build.props` sets net8.0 for all projects → change to net10.0
- `tests\Home.Blog.Common.Tests.csproj` and `tests\Home.Blog.Core.Tests.csproj` locally set net8.0 → change to net10.0

**Package actions (from assessment queries)**:
- Home.Blog.Common: Microsoft.Extensions.Configuration 8.0.0 → 10.0.11
- Home.Blog.Mvc:
  - Microsoft.Extensions.Caching.Memory 8.0.1 → 10.0.11
  - System.Formats.Asn1 6.0.1 → 10.0.11
  - System.Text.Json 8.0.5 → 10.0.11
  - Azure.Identity 1.11.4 → 1.21.0 (deprecated MSAL dependency)
  - System.IdentityModel.Tokens.Jwt 6.34.0 → 8.22.0 (deprecated)
  - Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.19.6 → no supported version found (verified with get_supported_package_version) → remove; document as deferred/VS container tooling note
  - Piranha.* 12.0.0 → 12.2.0 (latest supported, verified)
- Home.Blog.Data: Piranha 12.1.0 → 12.2.0 for consistency
- Test projects (Common.Tests, Core.Tests): align with Data.Tests versions — xunit 2.9.3, xunit.runner.visualstudio 3.1.5, Microsoft.NET.Test.Sdk 18.0.1, coverlet.collector 6.0.4. (xunit v2 → v3 migration deferred; keep v2 line consistent across tests.)

**Api.0003 behavioral changes** (Home.Blog.Common `Extensions\IConfigurationBuilderExtensions.cs`):
- `Environment.SetEnvironmentVariable` (line 59) and `JsonDocument.Parse` (line 32) — potential-severity behavioral notes; review code and confirm no action needed.
