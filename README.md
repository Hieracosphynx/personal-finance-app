# Personal Finance App [WIP]

> A vibe coding experiment powered by Claude AI — exploring the capabilities and limitations of AI-assisted development.

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

*To be documented as the project evolves.*

---

## License

MIT
