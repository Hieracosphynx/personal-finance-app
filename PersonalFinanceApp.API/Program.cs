using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=personalfinance;Username=hieracosphynx;Password=1007"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/accounts", async (AppDbContext db) =>
    {
        return await db.Accounts.ToListAsync();
    });
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Accounts.Any())
    {
        db.Accounts.AddRange(
            new Account { Institution = "Fidelity", AccountType = "Brokerage", Balance = 12345.00m, LastSynced = DateTime.UtcNow },
            new Account { Institution = "Ally Bank", AccountType = "Savings", Balance = 4215.50m, LastSynced = DateTime.UtcNow },
            new Account { Institution = "Wells Fargo", AccountType = "Checking", Balance = 1875.20m, LastSynced = DateTime.UtcNow },
            new Account { Institution = "One Nevada", AccountType = "Savings", Balance = 902.00m, LastSynced = DateTime.UtcNow }
        );
        await db.SaveChangesAsync();
    }
}
app.Run();
