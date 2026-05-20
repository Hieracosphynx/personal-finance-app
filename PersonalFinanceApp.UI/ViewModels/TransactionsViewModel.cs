using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using PersonalFinanceApp.Core.Models;

namespace PersonalFinanceApp.UI.ViewModels;

public partial class TransactionsViewModel : ViewModelBase
{
    public ObservableCollection<TransactionViewModel> Transactions { get; } = new();

    public async Task LoadTransactions(string plaidAccountId)
    {
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
}

public partial class TransactionViewModel : ViewModelBase
{
    public string Name { get; }
    public string Category { get; }
    public decimal Amount { get; }
    public DateOnly Date { get; }
    public string DisplayAmount => Amount.ToString("C");
    public string DisplayDate => Date.ToString("MMM dd");

    public TransactionViewModel(string name, string category, decimal amount, DateOnly date)
    {
        Name = name;
        Category = category;
        Amount = amount;
        Date = date;
    }
}
