namespace PersonalFinanceApp.Core.Models;

public class Transaction
{
    public int Id { get; set; }
    public string PlaidTransactionId { get; set; } = string.Empty;
    public string PlaidAccountId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
