using Going.Plaid;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddPlaid(builder.Configuration.GetSection("Plaid"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

// GET
app.MapGet("/accounts", async (AppDbContext db) =>
    await db.Accounts.ToListAsync());
app.MapGet("/plaid/connect", async (PlaidClient plaid) =>
{
    var linkToken = await CreateLinkToken(plaid);
    return Results.Redirect($"/link.html?link_token={linkToken}");
});
app.MapGet("/plaid/items", async (AppDbContext db) =>
    await db.PlaidItems.ToListAsync());

// POST
app.MapPost("/link-token", async (PlaidClient plaid) =>
{
    var linkToken = await CreateLinkToken(plaid);
    return Results.Ok(new { linkToken });
});
app.MapPost("/plaid/exchange-token", async (ExchangeRequest req, PlaidClient plaid, AppDbContext db) =>
{
    var response = await plaid.ItemPublicTokenExchangeAsync(new()
    {
        PublicToken = req.PublicToken
    });

    db.PlaidItems.Add(new PlaidItem
    {
        ItemId = response.ItemId,
        AccessToken = response.AccessToken,
        InstitutionName = req.InstitutionName,
        LinkedAt = DateTime.UtcNow
    });

    await db.SaveChangesAsync();
    return Results.Ok();
});
app.MapPost("/plaid/sync", async (PlaidClient plaid, AppDbContext db) =>
{
    var items = await db.PlaidItems.ToListAsync();

    foreach (var item in items)
    {
        var response = await plaid.AccountsBalanceGetAsync(new()
        {
            ClientId = builder.Configuration["Plaid:ClientId"],
            Secret = builder.Configuration["Plaid:Secret"],
            AccessToken = item.AccessToken
        });

        foreach (var account in response.Accounts)
        {
            var existing = await db.Accounts
              .FirstOrDefaultAsync(a => a.PlaidAccountId == account.AccountId);

            if (existing is null)
            {
                db.Accounts.Add(new Account
                {
                    PlaidAccountId = account.AccountId,
                    Institution = item.InstitutionName,
                    AccountType = account.Subtype.ToString() ?? "",
                    Balance = account.Balances.Current ?? 0,
                    LastSynced = DateTime.UtcNow
                });
            }
            else
            {
                existing.Balance = account.Balances.Current ?? 0;
                existing.LastSynced = DateTime.UtcNow;
            }
        }
    }

    await db.SaveChangesAsync();
    return Results.Ok();
});

app.UseStaticFiles();
app.Run();

async Task<string> CreateLinkToken(PlaidClient plaid)
{
    var response = await plaid.LinkTokenCreateAsync(new()
    {
        ClientId = builder.Configuration["Plaid:ClientId"],
        Secret = builder.Configuration["Plaid:Secret"],
        ClientName = "Personal Finance App",
        Language = Going.Plaid.Entity.Language.English,
        CountryCodes = [Going.Plaid.Entity.CountryCode.Us],
        User = new() { ClientUserId = "local-user" },
        Products = [Going.Plaid.Entity.Products.Transactions]
    });

    return response.LinkToken;
}

record ExchangeRequest(string PublicToken, string InstitutionName);
