# Contributing to The Mane Feed

Thank you for your interest in contributing! This guide covers the development workflow, conventions, and process for submitting changes.

## Development Setup

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) and npm
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or LocalDB
- A code editor (Visual Studio, VS Code, or Rider)

### First-time setup

```bash
git clone https://github.com/QuinntyneBrown/TheManeFeed.git
cd TheManeFeed
dotnet restore
dotnet build
```

For the Angular frontend:

```bash
cd src/TheManeFeed.Web
npm install
```

For the scraper CLI (installs Playwright browsers):

```bash
dotnet build src/TheManeFeed.Cli
pwsh src/TheManeFeed.Cli/bin/Debug/net8.0/playwright.ps1 install
```

## Project Layout

| Project | What it does | When to change |
|---------|-------------|----------------|
| `TheManeFeed.Core` | Domain entities and repository interfaces | Adding new entities or modifying the domain model |
| `TheManeFeed.Infrastructure` | EF Core DbContext, entity configurations, repository implementations | Database schema changes, new repositories, migrations |
| `TheManeFeed.Api` | ASP.NET Core REST API controllers and startup | New endpoints, middleware, API configuration |
| `TheManeFeed.Cli` | Playwright-based article scraper with site-specific strategies | Adding new scraper sources, fixing broken scrapers |
| `TheManeFeed.Web` | Angular SPA with shared component and API libraries | UI features, components, styling |
| `docs/design.pen` | UI design file (Pencil) | Design changes, new screens |

## Branching Strategy

1. Create a feature branch from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. Make your changes in small, focused commits.
3. Push and open a pull request against `main`.

### Branch naming

- `feature/` — new functionality
- `fix/` — bug fixes
- `refactor/` — code improvements without behavior changes
- `docs/` — documentation only

## Code Conventions

### .NET (API, Core, Infrastructure, CLI)

- Follow standard C# naming conventions (PascalCase for public members, camelCase for locals).
- New entities go in `TheManeFeed.Core`. Add a corresponding EF Core configuration class in `TheManeFeed.Infrastructure/Configurations/` and a repository in `TheManeFeed.Infrastructure/Repositories/`.
- Register new repositories in `InfrastructureServiceExtensions.cs`.
- Use constructor injection for dependencies.
- Log with Serilog's structured logging (`Log.Information("Processing {ArticleCount} articles", count)`).

### Angular (Web)

- Use standalone components (no NgModules).
- Shared UI components belong in `projects/components/`.
- API client services belong in `projects/api/`.
- Application-specific components and pages go in `projects/the-mane-feed/`.
- Use Angular signals for reactive state.
- Style with SCSS. Follow the design tokens defined in `docs/design.pen`.

### Database

- Entity configurations use EF Core Fluent API (not data annotations).
- Migrations auto-run on startup. To add a new migration:
  ```bash
  dotnet ef migrations add YourMigrationName --project src/TheManeFeed.Infrastructure --startup-project src/TheManeFeed.Api
  ```

## Adding a New Scraper

The CLI uses a strategy pattern for site-specific scrapers. To add a new source:

1. Create a new class in `src/TheManeFeed.Cli/Scrapers/` that extends `BaseSiteScraper`.
2. Implement the required scraping logic for the target site.
3. Register the scraper in the CLI's DI setup (`ServiceCollectionExtensions.cs`).
4. Add the source URL to `ScrapeSettings`.

## Design

The design file at `docs/design.pen` is the source of truth for UI. Open it with [Pencil](https://pencil.dev) to view and edit. When making frontend changes:

- Reference the design for spacing, colors, typography, and layout.
- The design uses these tokens: rose gold `#B76E79`, champagne `#F5E6D3`, cream `#FFF8F0`, dark `#1A1A1A`.
- Typography: Playfair Display (headlines), Inter (body/UI).
- Mobile-first responsive: 390px, 768px, 1440px breakpoints.

## Pull Request Process

1. Ensure the solution builds without errors: `dotnet build`
2. Verify the Angular app builds: `cd src/TheManeFeed.Web && npx ng build`
3. Write a clear PR title and description explaining **what** changed and **why**.
4. Link any related issues.
5. A maintainer will review and merge.

## Reporting Issues

Open an issue on GitHub with:

- A clear title describing the problem or request.
- Steps to reproduce (for bugs).
- Expected vs actual behavior.
- Screenshots if it's a UI issue.

## Code of Conduct

Be respectful and constructive. We're building something for hair lovers, so keep the vibes positive.
