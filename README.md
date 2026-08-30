# personal-blog

## Frontend Architecture — "Fable" Theme

The presentation layer lives entirely in `src/Home.Blog.Mvc` and is a custom, dependency-light theme for Piranha CMS (no Bootstrap, no jQuery).

### SCSS (ITCSS-inspired)

```
assets/scss/
  style.scss                  # entry point (compiled by gulp)
  abstracts/_tokens.scss      # design tokens: colors, type scale, spacing, radii, motion (CSS custom props on :root)
  abstracts/_mixins.scss      # breakpoints, focus-ring, visually-hidden, line-clamp
  base/_reset.scss            # reset + element defaults, reduced-motion support
  base/_typography.scss       # heading/body type rules
  layout/_shell.scss          # container, site header/nav, hero, footer, skip link
  components/_components.scss # buttons, post cards, tags, pagination, forms, comments
  components/_blocks.scss     # styles for every Piranha block display template
```

Build: `npm install` then `npx gulp min` (compiles + minifies to `wwwroot/assets/css/style.min.css`).

### JavaScript

`wwwroot/assets/js/site.js` — vanilla JS, progressive enhancement only (mobile nav toggle with ARIA state + Escape handling). Image galleries use pure CSS scroll-snap.

### View → Piranha content model mapping

| View | Piranha model | Notes |
| --- | --- | --- |
| `Views/Shared/_Layout.cshtml` | `IApplicationService` sitemap | Nav is driven by `WebApp.Site.Sitemap.ForUserAsync` — editable in the Manager |
| `Views/Cms/Page.cshtml` | `StandardPage` (`Page<T>`) | Hero (Title/Excerpt/PrimaryImage) + block stream |
| `Views/Cms/Post.cshtml` | `StandardPost` (`Post<T>`) | Category, tags, blocks, comments (posts to `{permalink}/comment`) |
| `Views/Cms/Archive.cshtml` | `StandardArchive` (`PostArchive<PostInfo>`) | Card grid + accessible pagination, category/year/month filters preserved |
| `Views/Cms/DisplayTemplates/*.cshtml` | `Piranha.Extend.Blocks.*` | One partial per block type; resolved via `Html.DisplayFor(m => block, block.GetType().Name)` |
| `Views/Setup/Index.cshtml` | — | Standalone first-run page |

SEO/OG meta tags are rendered by `@WebApp.MetaTags(Model)` in each template's `head` section. All manager-editable regions, block types, and comment functionality are unchanged — content editors need no code changes.
