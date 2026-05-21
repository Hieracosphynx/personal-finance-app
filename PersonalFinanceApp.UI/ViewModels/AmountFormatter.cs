using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PersonalFinanceApp.UI.ViewModels;

public partial class AmountFormatter : ObservableObject
{
    public static AmountFormatter Instance { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowAmounts))]
    private bool _hideAmounts;

    private const string Mask = "••••••";

    public static string Format(decimal amount, string format = "C") =>
        Instance.HideAmounts ? Mask : amount.ToString(format);

    public static string FormatSigned(decimal amount) =>
        Instance.HideAmounts ? Mask : amount >= 0 ? $"-{amount:C}" : $"+{(-amount):C}";

    public bool ShowAmounts => !HideAmounts;

    public void Subscribe(string propertyName, Action onChanged)
    {
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == propertyName)
                onChanged();
        };
    }
}
