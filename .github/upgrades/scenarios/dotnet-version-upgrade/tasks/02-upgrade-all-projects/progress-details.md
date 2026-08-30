# 02-upgrade-all-projects — Progress Details

## Result: ✅ Complete — solution builds with 0 errors / 0 warnings on net10.0

## TFM Changes
- `Directory.Build.props`: net8.0 → net10.0 (central, covers all 7 projects)
- `tests\Home.Blog.Common.Tests\Home.Blog.Common.Tests.csproj`: local TFM net8.0 → net10.0
- `tests\Home.Blog.Core.Tests\Home.Blog.Core.Tests.csproj`: local TFM net8.0 → net10.0

## Package Updates
| Project | Package | From → To |
|---|---|---|
| Common | Microsoft.Extensions.Configuration | 8.0.0 → 10.0.11 |
| Data | Piranha | 12.1.0 → 12.2.0 |
| Mvc | Azure.Identity (deprecated) | 1.11.4 → 1.21.0 |
| Mvc | System.IdentityModel.Tokens.Jwt (deprecated) | 6.34.0 → 8.22.0 |
| Mvc | Piranha.* (8 packages) | 12.0.0 → 12.2.0 |
| Mvc | SixLabors.ImageSharp | 2.1.11 → 2.1.13 (NU1605 fix) |
| Mvc | Microsoft.Data.SqlClient | 5.1.3 → 5.1.6 (NU1605 fix) |
| Mvc | Microsoft.VisualStudio.Azure.Containers.Tools.Targets | **removed** — no net10.0-supported version exists (design-time VS container tooling only; Docker builds via Dockerfile unaffected) |
| Mvc | System.Text.Json, System.Formats.Asn1, Microsoft.Extensions.Caching.Memory | **removed** — framework-provided in .NET 10 (NU1510) |
| Common.Tests / Core.Tests | xunit 2.5.3 → 2.9.3; runner 2.5.3 → 3.1.5; Test.Sdk 17.8.0 → 18.0.1; coverlet 6.0.0 → 6.0.4 (aligned with Data.Tests) |

## Code Fixes (all build warnings resolved)
- `Common\Extensions\EnvironmentExtensions.cs`: nullable-annotated generic signatures (CS8600/03/04)
- `Common\Extensions\IConfigurationBuilderExtensions.cs`: `string?` optional parameter (CS8625)
- `Data\Storage\MinIO\MinioStorage.cs`: `string?` return on GetPublicUrl (CS8603)
- `Data\Storage\MinIO\MinioStorageSession.cs`: removed unused `_disposed` field (CS0169)
- `Mvc\Models\SaveCommentModel.cs`: initialized string properties (CS8618)
- `Mvc\Models\StandardArchive.cs`: initialized Archive property (CS8618)
- `Mvc\Views\Cms\Post.cshtml` / `Archive.cshtml`: guarded `Published.Value` with HasValue (CS8629)
- `Mvc\Settings\BaseSettings.cs`: null-forgiving on required getter (CS8603)

## Assessment Api.0003 (behavioral changes)
Reviewed `IConfigurationBuilderExtensions.cs` usage of `JsonDocument.Parse` and `Environment.SetEnvironmentVariable` — potential-severity behavioral notes only; usage patterns unaffected by .NET 10 changes. No code change required.

## Build
`dotnet build Home.Blog.sln --no-incremental` → Build succeeded, 0 Warning(s), 0 Error(s).
