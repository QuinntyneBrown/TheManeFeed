# The Mane Feed

A curated hair news aggregator that surfaces the top stories, trends, and innovations in hair — built for the style-obsessed.

## Overview

The Mane Feed aggregates articles from leading beauty and hair publications (Allure, Byrdie, Cosmo, Glamour, Vogue, and more), presenting them in a clean editorial feed. Users can browse by category, follow trending topics, save articles, and build personal collections.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| **API** | ASP.NET Core 8.0, Serilog |
| **Domain** | .NET 8.0 (clean architecture) |
| **Data** | Entity Framework Core 8.0, SQL Server |
| **Scraper CLI** | .NET 8.0, Playwright, System.CommandLine |
| **Web Frontend** | Angular 21, TypeScript 5.9, SCSS |
| **Design** | Pencil (.pen) — see `docs/design.pen` |

## Project Structure

```
TheManeFeed.sln
├── src/
│   ├── TheManeFeed.Core/           # Domain entities & repository interfaces
│   ├── TheManeFeed.Infrastructure/ # EF Core DbContext, repositories, migrations
│   ├── TheManeFeed.Api/            # ASP.NET Core REST API
│   ├── TheManeFeed.Cli/            # Playwright-based article scraper
│   └── TheManeFeed.Web/            # Angular SPA
│       ├── projects/api/           #   HTTP client library
│       ├── projects/components/    #   Shared UI component library
│       └── projects/the-mane-feed/ #   Main application
├── docs/
│   └── design.pen                  # Full UI design (mobile, tablet, desktop)
└── .playwright/
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) and npm
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or LocalDB)
- [Playwright browsers](https://playwright.dev/dotnet/docs/intro) (for the scraper CLI)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/QuinntyneBrown/TheManeFeed.git
cd TheManeFeed
```

### 2. Start the API

```bash
dotnet restore
dotnet build
dotnet run --project src/TheManeFeed.Api
```

The API auto-runs EF Core migrations on startup. By default it connects to LocalDB — update `src/TheManeFeed.Api/appsettings.json` to point at your SQL Server instance if needed.

### 3. Run the scraper

```bash
dotnet run --project src/TheManeFeed.Cli -- scrape
```

This launches Playwright, visits configured beauty/hair publication sites, and persists scraped articles to the database.

### 4. Start the frontend

```bash
cd src/TheManeFeed.Web
npm install
npx ng serve
```

The Angular app runs at `http://localhost:4200` by default and expects the API at the URL configured in the environment files.

## Architecture

The solution follows **clean/layered architecture**:

```
TheManeFeed.Api
├── TheManeFeed.Core           (domain entities, interfaces)
└── TheManeFeed.Infrastructure (EF Core, repositories)

TheManeFeed.Cli
├── TheManeFeed.Core
└── TheManeFeed.Infrastructure

TheManeFeed.Web  (standalone Angular SPA)
```

**Key patterns:** Repository pattern, dependency injection, strategy pattern (site-specific scrapers), standalone Angular components with signals.

## Design

The full UI design lives in `docs/design.pen` and can be opened with [Pencil](https://pencil.dev). It includes:

- **11 reusable components** — Logo, navigation, cards, search bar, footer, etc.
- **5 mobile screens** (390px) — Home Feed, Article Detail, Explore, Search, Profile
- **2 tablet screens** (768px) — Home Feed, Article Detail
- **2 desktop screens** (1440px) — Home Feed, Article Detail

Design tokens use a warm luxury editorial palette (rose gold, champagne, cream) with Playfair Display + Inter typography.

## License

See [LICENSE](LICENSE) for details.
