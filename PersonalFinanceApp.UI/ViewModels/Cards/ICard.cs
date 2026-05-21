using System.Threading.Tasks;

namespace PersonalFinanceApp.UI.ViewModels.Cards;

public interface ICard
{
    string Title { get; }
    Task LoadAsync();
}
