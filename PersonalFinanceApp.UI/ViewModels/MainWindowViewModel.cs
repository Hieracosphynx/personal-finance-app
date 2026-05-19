using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using PersonalFinanceApp.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PersonalFinanceApp.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public List<AccountViewModel> Accounts { get; } = new()
    {
        new AccountViewModel("Fidelity", "Brokerage", 12340.00m),
        new AccountViewModel("Ally Bank", "Savings", 4210.50m),
        new AccountViewModel("Wells Fargo", "Checking", 1875.20m),
        new AccountViewModel("One Nevada", "Savings", 900.00m),
    };

    public MainWindowViewModel()
    {

    }

    private async void LoadAccounts()
    {
        using var http = new HttpClient();
        var accounts = await http.GetFromJsonAsync<List<Account>>("http://localhost:5101/accounts");

        if (accounts is null) return;

        foreach (var account in accounts)
        {
            Accounts.Add(
                new AccountViewModel(
                  account.Institution,
                  account.AccountType,
                  account.Balance));
        }
    }
}

public partial class AccountViewModel : ViewModelBase
{
    public string Institution { get; }
    public string AccountType { get; }
    public decimal Balance { get; }
    public string DisplayName => $"{Institution} - {AccountType}";
    public string DisplayBalance => Balance.ToString("C");

    public AccountViewModel(string institution, string accountType, decimal balance)
    {
        Institution = institution;
        AccountType = accountType;
        Balance = balance;
    }
}
