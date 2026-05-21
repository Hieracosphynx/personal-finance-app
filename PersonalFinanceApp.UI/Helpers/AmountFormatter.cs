namespace PersonalFinanceApp.UI.Helpers;

public static class AmountFormatter
{
    public static string Format(decimal amount, bool hide, string format = "C") =>
        hide ? "••••••" : amount.ToString(format);

    public static string FormatSigned(decimal amount, bool hide) =>
        hide ? "••••••" : amount >= 0 ? $"-{amount:C}" : $"+{(-amount):C}";
}
