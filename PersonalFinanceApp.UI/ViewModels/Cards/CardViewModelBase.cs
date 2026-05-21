using System.Threading.Tasks;

namespace PersonalFinanceApp.UI.ViewModels.Cards;

public abstract class CardViewModelBase : ViewModelBase, ICard
{
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        protected set => SetProperty(ref _title, value);
    }

    public abstract Task LoadAsync();
}
