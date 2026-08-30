# .NET Version Upgrade

## Strategy
**Selected**: All-At-Once
**Rationale**: 7 projects, all net8.0, SDK-style, shallow dependency graph, no high-risk migrations.

### Execution Constraints
- Single atomic upgrade — all projects updated together (TFMs, packages, code fixes) in one pass
- No tier ordering; build and fix all compilation errors in one bounded pass
- Testing runs after the atomic upgrade completes successfully
- Validate full solution build with 0 errors before final validation task

## Upgrade Options
- **Upgrade Strategy**: All-at-Once

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0 (LTS)

### Technical Preferences
- **AutoMapper**: Pinned to exactly `[12.0.1]` in Home.Blog.Mvc — required by Piranha.Data.EF 12.2.0 (AutoMapper 13+ breaks Piranha startup). NU1903 advisory suppressed per user approval (2025); revisit when Piranha updates AutoMapper.

## Source Control
- **Source Branch**: redesign/chatgpt-prompt
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)
