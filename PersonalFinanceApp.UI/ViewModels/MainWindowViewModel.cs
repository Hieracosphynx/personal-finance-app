namespace PersonalFinanceApp.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public AccountsViewModel Accounts { get; } = new();
    public TransactionsViewModel Transactions { get; } = new();

    public MainWindowViewModel()
    {
        Accounts.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AccountsViewModel.SelectedAccount)
                && Accounts.SelectedAccount is not null)
                _ = Transactions.LoadTransactions(Accounts.SelectedAccount.PlaidAccountId);
        };
    }
}
