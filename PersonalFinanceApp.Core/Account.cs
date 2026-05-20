namespace PersonalFinanceApp.Core.Models;

public class Account
{
    public string PlaidAccountId { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime LastSynced { get; set; }
}
