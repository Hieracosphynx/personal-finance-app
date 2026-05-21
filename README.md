# Personal Finance App [WIP]

> A vibe coding experiment powered by Claude AI — exploring the capabilities and limitations of AI-assisted development.

![Demo](docs/images/personalfinanceapp.gif)

---

## Purpose

This project is built to:
- Use an AI-assisted workflow: **Prompt -> Read -> Modify -> Test -> Prompt**
- Explore the **capabilities and limitations** of AI in a real-world project
- Build a functional personal finance dashboard along the way

---

## Tech Stack

| Layer | Technology |
|---|---|
| **UI** | [Avalonia UI](https://avaloniaui.net/) — cross-platform desktop UI framework for .NET |
| **Database** | [PostgreSQL](https://www.postgresql.org/) — reliable, open-source relational database |
| **Bank Data** | [Plaid](https://plaid.com/) — API for connecting to financial institutions |

---

## Dependencies
  | Package | Version | Notes |
  |---|---|---|
  | [Avalonia](https://avaloniaui.net/) | 12.0.3 | UI framework |
  | [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) | 8.4.1 | MVVM source generators |
  | [LiveChartsCore.SkiaSharpView.Avalonia](https://livecharts.dev/) | 2.1.0-dev-570 | ⚠️ Prerelease — required for Avalonia 12 compatibility,
   stable 2.0.4 does not support Avalonia 12 |
  | [Going.Plaid](https://github.com/viceroypenguin/Going.Plaid) | latest | Plaid API client |
  | [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.npgsql.org/efcore/) | latest | PostgreSQL + EF Core |

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- Plaid developer account (free at [plaid.com](https://plaid.com))

### Run the App
```bash
# Restore dependencies
dotnet restore

# Run the UI
dotnet run --project PersonalFinanceApp.UI
```

---

## Project Structure

```
PersonalFinanceApp/
├── PersonalFinanceApp.UI/       # Avalonia frontend
├── PersonalFinanceApp.API/      # ASP.NET Core backend
├── PersonalFinanceApp.Core/     # Shared models & business logic
└── PersonalFinanceApp.Data/     # EF Core + PostgreSQL
```

---

## AI Development Notes
  **What went well**
  - Quickly scaffolded the full solution structure (UI, API, Core, Data)
  - Explained Avalonia/MVVM concepts from scratch without assuming prior knowledge
  - Caught the orphaned root-level duplicate project before it caused confusion
  - Identified the List<T> → ObservableCollection<T> bug before you ran it
  - Spotted LoadAccounts never being called
  - Kept pace with your stack decisions (Plaid, PostgreSQL, .NET 9)
  - Plaid link token working end to end
  - Full Plaid Link flow working — access token saved to database
  - Transactions syncing and displaying correctly
  - Clean two-panel layout with separate Views and ViewModels
  - Proactively suggested separating Views and ViewModels into subfolders
  - Explained event subscription (+=) and MVVM patterns clearly
  - Caught Cards/ being placed in Core when it belongs in UI

  **Limitations / friction points**
  - Can't execute JavaScript, so couldn't fetch your shared Claude chat directly
  - No AXAML-aware LSP to recommend for Neovim — that tooling gap is a real one
  - Can't run the app to verify things visually
  - The id → Id rename caused a migration mismatch — could have caught that earlier
  - Gave wrong AddPlaid lambda syntax initially, causing unnecessary back and forth
  - Incorrectly diagnosed the Plaid credentials issue — fix was ClientId/Secret on the request
  - Gave wrong fix for the onSuccess async issue — actual problem was a typo

---

## License

MIT
