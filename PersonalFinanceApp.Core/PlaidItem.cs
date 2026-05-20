namespace PersonalFinanceApp.Core.Models;

public class PlaidItem
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public DateTime LinkedAt { get; set; }
}
