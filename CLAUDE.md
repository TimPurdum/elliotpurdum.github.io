# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

`elliotpurdum.github.io` (deployed at https://www.elliotpurdum.com) is a Blazor WebAssembly portfolio + blog for Elliot Purdum, a cellist and conductor. Pages are pre-rendered to static HTML at build time; the WASM runtime hydrates them on load. Same hybrid SSG + interactive WASM model as `timpurdum.github.io`, sharing the BlogGenerator engine.

Target framework: **.NET 10**. C# `latest`, nullable enabled.

## Submodules — clone recursively

```
BlogGenerator/  (https://github.com/TimPurdum/BlogGenerator.git, version 1.1.0+)
BlogGenerator/markdig/  (custom Markdig fork)
```

Always clone with `--recurse-submodules` (or run `git submodule update --init --recursive` after a fresh clone). CI does this via `actions/checkout@v4` with `submodules: recursive`.

## Solution layout

Three project groups; same chain pattern as timpurdum.dev:

1. **`ElliotPurdum.com.Source`** — Razor class library with the *content* and templates.
   - `Content/Posts/YYYY-MM-DD-slug.md` — blog posts.
   - `Content/Pages/*.razor` — homepage, About, Music, Shows, Gallery, Blog index. Each declares `@page "/route"`.
   - `Content/Music/YYYY-MM-DD-slug.md` — music portfolio entries (performances, recordings, compositions). Filename date is performance/recording date metadata; URLs are flat `/music/{slug}.html`.
   - `Content/Shows/YYYY-MM-DD-slug.md` — concert / event entries. Filename date IS the performance date and is used in the URL `/show/YYYY/M/D/{slug}.html`.
   - `Content/Gallery/YYYY-MM-slug.md` — photo gallery collections (frontmatter has nested `images:` list with `src` + `caption`). Flat URLs `/gallery/{slug}.html`.
   - `Templates/` — site-specific Razor templates that override BlogGenerator's defaults: `RootTemplate`, `PageLayout`, `PostLayout`, `MusicLayout`, `ShowLayout`, `GalleryLayout`, `Header`, `Footer`, `NavMenu`.
   - Built first; its compiled DLL is loaded reflectively by the BlogGenerator Compiler.

2. **`BlogGenerator/`** (submodule) — the static site generator.
   - `TimPurdum.Dev.BlogGenerator.Compiler` runs `BeforeTargets="Build"` on the WASM project, reading markdown + razor pages and emitting static HTML into `ElliotPurdum.com/wwwroot/`. It also generates `.razor` components for any embedded `blazor-component` code blocks.
   - `TimPurdum.Dev.BlogGenerator.Shared` exposes `BlogSettings`, the abstract layout base classes (`BaseRootTemplate`, `BasePostLayout`, `BaseMusicLayout`, etc.), and the metadata records (`MusicMetaData`, `ShowMetaData`, `GalleryMetaData`).
   - Reflective component discovery wires interactive components into the pre-rendered HTML at runtime via `WebAssemblyHostBuilder.AddGeneratedBlogContent()`.

3. **`ElliotPurdum.com`** — the Blazor WASM project that ships. References `BlogGenerator` (which transitively triggers the generator) and `ElliotPurdum.com.Source`. `Program.cs` is intentionally tiny.

### Build order

`ElliotPurdum.com` cannot be built standalone from a clean state — the Compiler needs `ElliotPurdum.com.Source.dll` on disk to load templates reflectively. CI builds them in order:

```bash
cd ElliotPurdum.com.Source && dotnet build -c Release
cd BlogGenerator/TimPurdum.Dev.BlogGenerator.Compiler && dotnet run -c Release
cd ElliotPurdum.com && dotnet publish -c Release
```

Locally `dotnet build ElliotPurdum.com.slnx` usually works because project references trigger Source first and the BlogGenerator targets file runs the Compiler.

## URL conventions

`MusicMetaData.Url`, `ShowMetaData.Url`, `GalleryMetaData.Url`, `PostMetaData.Url` are stored **extensionless** (e.g. `/music/foo`, `/show/2026/9/12/foo`). Consumers append `.html` at href-render time — see `Index.razor`, `Music.razor`, etc. The on-disk files at `OutputPath` retain the `.html` extension.

## PageTitle handling — gotcha

BlogGenerator's parser does NOT evaluate Razor expressions inside `<PageTitle>...</PageTitle>`. Two options work:

1. **Hardcoded literal** (used here): `<PageTitle>Music — Elliot Purdum</PageTitle>`. The parser captures the text verbatim and uses it as both the document title and the page's `Title` parameter.
2. **Special-cased substitution**: `<PageTitle>@BlogSettings.SiteTitle</PageTitle>` — the parser substitutes the site title at compile time. Requires `[Inject] BlogSettings BlogSettings` on the page.

Don't use `<PageTitle>@SomeOtherProperty</PageTitle>` — the literal `@SomeOtherProperty` will end up in the rendered `<title>` tag.

## Line endings — gotcha

BlogGenerator's parser splits files on `Environment.NewLine`, which is platform-dependent. Files in this repo should use **CRLF** on Windows (or LF if working from Linux exclusively). Mixed line endings cause the parser to see the entire file as a single line and fail to strip `<PageTitle>` / component tags. Until BlogGenerator's parser is patched to handle both, keep line endings consistent.

The `.gitattributes` file (if present) and your local `git config core.autocrlf` settings should be aligned with this.

## Configuration

- `ElliotPurdum.com/wwwroot/appsettings.json` is **checked in**. The site has no third-party secrets (no API keys, no GeoBlazor license) — the file holds only public `BlogSettings` (site name, URL, content paths). Edit it directly when you need to tweak settings.
- The `BlogSettings` paths are resolved relative to the Source project folder by the Compiler's `Program.cs` before generation runs. Don't hardcode absolute paths.
- If a future feature ever needs a secret, switch back to a gitignored `appsettings.json` + a CI secret-injection step (the old workflow pattern is preserved in git history).

## NuGet feeds

`NuGet.Config` pins three sources: `dotnet10` Azure DevOps feed (for .NET 10 packages), `NuGet.org`, and `local`. If a restore fails for `Microsoft.*` 10.0.x packages, the dotnet10 feed is the likely cause.

## Common commands

```bash
dotnet build ElliotPurdum.com.slnx         # full build (chain: Source → Compiler → WASM)
cd ElliotPurdum.com && dotnet run          # local dev server (after at least one full build)
# Re-run the generator only (after editing posts/templates):
cd BlogGenerator/TimPurdum.Dev.BlogGenerator.Compiler && dotnet run -c Release
cd ElliotPurdum.com && dotnet publish -c Release   # production publish
```

There are no unit tests in this repo.

## Authoring content

- **New post** → `ElliotPurdum.com.Source/Content/Posts/YYYY-MM-DD-slug.md` with YAML frontmatter (`layout: post`, `title`, `subtitle?`, `description?`, `lastmodified?`).
- **New music entry** → `Content/Music/YYYY-MM-DD-slug.md`, frontmatter: `layout: music`, `title`, `type` (performance | recording | composition), `ensemble`, `role`, `venue`, `embedUrl?`, `coverImage?`, `description?`. Filename date = performance/recording date metadata. **Slug must be unique within music** — collisions hard-fail the build.
- **New show entry** → `Content/Shows/YYYY-MM-DD-slug.md`, frontmatter: `layout: show`, `title`, `time` (free-form like "7:30 PM"), `venue`, `city`, `ticketUrl?`, `program?`, `role?`, `description?`. Filename date = performance date and is reflected in the URL.
- **New gallery** → `Content/Gallery/YYYY-MM-slug.md`, frontmatter: `layout: gallery`, `title`, `description?`, `images:` (indented YAML list of `src`/`caption` mappings). **Slug must be unique within gallery**.
- **Edit a page** → `Content/Pages/*.razor`. Add `@page "/route"` directive. Available `[Parameter]` properties: `NavLinks`, `MusicEntries`, `ShowEntries`, `GalleryEntries`, plus a few BlogSettings fields.

After editing, re-run the Compiler (or rebuild the solution) — generated HTML lives under `ElliotPurdum.com/wwwroot/`. Generated HTML is checked in (the deployed site serves the static files directly).

## Deployment

`.github/workflows/static.yml` builds on push to `main` and deploys `ElliotPurdum.com/bin/Release/net10.0/publish/wwwroot` to GitHub Pages. The custom domain `elliotpurdum.com` is set via `wwwroot/CNAME`.

To deploy: push to `main`. The workflow handles the rest. Custom domain DNS at the registrar must point to GitHub Pages (CNAME `elliotpurdum.com` → `timpurdum.github.io.` style, or A records to GitHub Pages IPs).
