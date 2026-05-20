using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Core.Models;

namespace PersonalFinanceApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<PlaidItem> PlaidItems { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
