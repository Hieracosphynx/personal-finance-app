using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using PersonalFinanceApp.Core.Models;
using CommunityToolkit.Mvvm.Input;

namespace PersonalFinanceApp.UI.ViewModels;

public partial class AccountsViewModel : ViewModelBase
{
    private AccountViewModel? _selectedAccount;
    public AccountViewModel? SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            SetProperty(ref _selectedAccount, value);
        }
    }

    public ObservableCollection<AccountViewModel> Accounts { get; } = new();

    public AccountsViewModel()
    {
        _ = LoadAccounts();
    }

    [RelayCommand]
    private async Task Sync()
    {
        using var http = new HttpClient();
        await http.PostAsync("http://localhost:5101/plaid/sync", null);

        Accounts.Clear();
        await LoadAccounts();
    }

    private async Task LoadAccounts()
    {
        using var http = new HttpClient();
        var accounts = await http.GetFromJsonAsync<List<Account>>("http://localhost:5101/accounts");

        if (accounts is null) return;

        foreach (var account in accounts)
        {
            Accounts.Add(new AccountViewModel(
               account.Institution,
               account.AccountType,
               account.Balance,
               account.PlaidAccountId
            ));
        }
    }
}

public partial class AccountViewModel : ViewModelBase
{
    public string PlaidAccountId { get; }
    public string Institution { get; }
    public string AccountType { get; }
    public decimal Balance { get; }
    public string DisplayName => $"{Institution} - {AccountType}";
    public string DisplayBalance => Balance.ToString("C");

    public AccountViewModel(string institution, string accountType, decimal balance, string plaidAccountId)
    {
        Institution = institution;
        AccountType = accountType;
        Balance = balance;
        PlaidAccountId = plaidAccountId;
    }
}

