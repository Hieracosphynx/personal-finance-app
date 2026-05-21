using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using PersonalFinanceApp.Core.Models;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;

namespace PersonalFinanceApp.UI.ViewModels;

public partial class TransactionsViewModel : ViewModelBase
{
    public ObservableCollection<TransactionViewModel> Transactions { get; } = new();
    private string _currentPlaidAccountId = string.Empty;

    public async Task LoadTransactions(string plaidAccountId)
    {
        _currentPlaidAccountId = plaidAccountId;
        Transactions.Clear();
        using var http = new HttpClient();
        var transactions = await http.GetFromJsonAsync<List<Transaction>>(
            $"http://localhost:5101/transactions/{plaidAccountId}");

        if (transactions is null) return;

        foreach (var t in transactions)
        {
            Transactions.Add(new TransactionViewModel(t.Name, t.Category, t.Amount, t.Date));
        }
    }

    [RelayCommand]
    private async Task Sync()
    {
        using var http = new HttpClient();
        await http.PostAsync("http://localhost:5101/plaid/sync/transactions", null);
        if (!string.IsNullOrEmpty(_currentPlaidAccountId))
            await LoadTransactions(_currentPlaidAccountId);
    }
}

public partial class TransactionViewModel : ViewModelBase
{
    public string Name { get; }
    public string Category { get; }
    public decimal Amount { get; }
    public DateOnly Date { get; }
    public string DisplayDate => Date.ToString("MMM dd");

    public string DisplayAmount => Amount >= 0
    ? $"-{Amount.ToString("C")}"
    : $"+{(-Amount).ToString("C")}";
    public IBrush AmountColor => Amount >= 0 ? Brushes.Red : Brushes.Green;

    public TransactionViewModel(string name, string category, decimal amount, DateOnly date)
    {
        Name = name;
        Category = category;
        Amount = amount;
        Date = date;
    }
}
