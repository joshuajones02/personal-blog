# 01-prerequisites: Verify SDK and toolchain readiness

Verify the .NET 10 SDK is installed and check for any global.json that pins an older SDK version. Confirm Directory.Build.props / Directory.Packages.props conventions used by the repo so TFM and package updates are made in the right place.

**Done when**: .NET 10 SDK confirmed installed; global.json (if any) compatible with .NET 10.

## Research Findings

- .NET 10 SDK confirmed installed (validate_dotnet_sdk_installation: "Compatible SDK found")
- No global.json in the repo — no SDK pinning
- TFM is centralized in root `Directory.Build.props` (`net8.0`), imported by `src\Directory.Build.props` and `tests\Directory.Build.props`
- Two test projects override TFM locally: `Home.Blog.Core.Tests.csproj` and `Home.Blog.Common.Tests.csproj` (both net8.0) — must be updated in task 02 along with the root props
- No Directory.Packages.props — packages managed per-project
