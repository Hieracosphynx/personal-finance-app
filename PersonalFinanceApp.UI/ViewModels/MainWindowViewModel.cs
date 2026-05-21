using PersonalFinanceApp.UI.ViewModels.Cards;

namespace PersonalFinanceApp.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public AccountsViewModel Accounts { get; } = new();
    public TransactionsViewModel Transactions { get; } = new();
    public LineGraphCardViewModel LineGraph { get; } = new();
    public AmountFormatter AmountFormatter => AmountFormatter.Instance;

    public MainWindowViewModel()
    {
        Accounts.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AccountsViewModel.SelectedAccount)
                && Accounts.SelectedAccount is not null)
            {
                var plaidAccountId = Accounts.SelectedAccount.PlaidAccountId;
                _ = Transactions.LoadTransactions(plaidAccountId);
                _ = LineGraph.LoadHistorical(plaidAccountId, Accounts.SelectedAccount.Balance);
            }
        };
    }
}
