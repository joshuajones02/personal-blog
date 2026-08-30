# .NET 10 Upgrade Plan

## Overview

**Target**: Upgrade Home.Blog solution from .NET 8 to .NET 10 (LTS)
**Scope**: 7 projects (4 source, 3 test), all SDK-style, all on net8.0. 20 assessment issues (8 mandatory). Small solution, shallow dependency graph.

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 7 projects, all on .NET 8, SDK-style, clear dependency structure, no high-risk package migrations.

## Tasks

### 01-prerequisites: Verify SDK and toolchain readiness

Verify the .NET 10 SDK is installed and check for any global.json that pins an older SDK version. Confirm Directory.Build.props / Directory.Packages.props conventions used by the repo so TFM and package updates are made in the right place.

**Done when**: .NET 10 SDK confirmed installed; global.json (if any) compatible with .NET 10.

---

### 02-upgrade-all-projects: Upgrade all projects to net10.0 and update packages

Update TargetFramework from net8.0 to net10.0 across all 7 projects (Home.Blog.Common, Home.Blog.Core, Home.Blog.Data, Home.Blog.Mvc, and the three test projects). Bump recommended packages: Microsoft.Extensions.Caching.Memory, Microsoft.Extensions.Configuration, System.Formats.Asn1, System.Text.Json → 10.0.x, and align other Microsoft.Extensions.*/EF Core/ASP.NET Core packages (currently 8.0.x) to 10.0.x. Address flagged packages: Microsoft.VisualStudio.Azure.Containers.Tools.Targets (incompatible — update or remove), deprecated Azure.Identity 1.11.4 (update to current), System.IdentityModel.Tokens.Jwt (transitive/deprecated — verify), and deprecated xunit 2.x (update to current supported version). Assessment flagged one behavioral-change API issue in Home.Blog.Common — investigate and fix during the pass. Restore, build the full solution, and fix all compilation errors and warnings in a single bounded pass.

Research starting points: check Directory.Build.props for centralized TFM; verify Piranha CMS 12.x compatibility with net10.0 before bumping ASP.NET Core packages; review the Api.0003 behavioral change detail in assessment.md.

**Done when**: All 7 projects target net10.0; recommended package updates applied; solution builds with 0 errors and 0 warnings in modified projects.

---

### 03-final-validation: Full solution validation

Run a full solution build and execute all test suites (Home.Blog.Common.Tests, Home.Blog.Core.Tests, Home.Blog.Data.Tests). Document any deferred recommendations.

**Done when**: Solution builds clean; all tests pass.
