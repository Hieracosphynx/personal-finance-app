using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using PersonalFinanceApp.Core.Models;
using SkiaSharp;

namespace PersonalFinanceApp.UI.ViewModels.Cards;

public partial class LineGraphCardViewModel : CardViewModelBase
{
    private ISeries[] _series = [];
    public ISeries[] Series
    {
        get => _series;
        private set => SetProperty(ref _series, value);
    }

    public Axis[] XAxes { get; } =
    [
        new DateTimeAxis(TimeSpan.FromDays(30), d => d.ToString("MMM yyyy"))
    ];
    public Axis[] YAxes { get; } =
    [
        new Axis { Labeler = value => value.ToString("C0") }
    ];

    public string DisplayBalance => CurrentBalance.ToString("C");
    public string DisplayTotalSaved => TotalSaved.ToString("C0");
    public string DisplayTotalGains => TotalGains.ToString("C0");

    public bool IsYears10 => ProjectionYears == 10;
    public bool IsYears20 => ProjectionYears == 20;
    public bool IsYears30 => ProjectionYears == 30;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayBalance))]
    private decimal _currentBalance;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayTotalSaved))]
    private decimal _totalSaved;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayTotalGains))]
    private decimal _totalGains;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsYears10))]
    [NotifyPropertyChangedFor(nameof(IsYears20))]
    [NotifyPropertyChangedFor(nameof(IsYears30))]
    private int _projectionYears = 10;

    [ObservableProperty] private string _startingBalanceInput = "0";
    [ObservableProperty] private string _savingsInput = "0";
    [ObservableProperty] private string _rateInput = "7";
    [ObservableProperty] private bool _showHistorical = false;
    [ObservableProperty] private bool _showProjection = true;

    private List<DateTimePoint> _historicalPoints = [];
    private List<DateTimePoint> _projectionPoints = [];

    public LineGraphCardViewModel()
    {
        Title = "Balance Over Time";
    }

    public override Task LoadAsync() => Task.CompletedTask;

    partial void OnShowProjectionChanged(bool value) => UpdateSeries();
    partial void OnCurrentBalanceChanged(decimal value) =>
      StartingBalanceInput = value.ToString("F2");
    partial void OnShowHistoricalChanged(bool value)
    {
        if (value) StartingBalanceInput = CurrentBalance.ToString("F2");
        UpdateSeries();
    }

    public async Task LoadHistorical(string plaidAccountId, decimal currentBalance)
    {
        CurrentBalance = currentBalance;
        ShowHistorical = true;

        _historicalPoints.Clear();

        using var http = new HttpClient();
        var transactions = await http.GetFromJsonAsync<List<Transaction>>(
            $"http://localhost:5101/transactions/{plaidAccountId}");

        if (transactions is not null && transactions.Count > 0)
        {
            var sorted = transactions.OrderByDescending(t => t.Date).ToList();
            var balance = (double)currentBalance;

            _historicalPoints.Add(new DateTimePoint(DateTime.Today, balance));

            foreach (var t in sorted)
            {
                balance += (double)t.Amount;
                _historicalPoints.Add(new DateTimePoint(t.Date.ToDateTime(TimeOnly.MinValue), balance));
            }

            _historicalPoints.Reverse();
        }

        RecalculateProjection();
        UpdateSeries();
    }

    [RelayCommand]
    private void ApplyInputs()
    {
        RecalculateProjection();
        UpdateSeries();
    }

    [RelayCommand]
    private void SetYears(string years)
    {
        if (int.TryParse(years, out var y))
            ProjectionYears = y;
        RecalculateProjection();
        UpdateSeries();
    }

    private void RecalculateProjection()
    {
        var monthlySavings = decimal.TryParse(SavingsInput, out var s) ? s : 0m;
        var annualRate = double.TryParse(RateInput, out var r) ? r : 7.0;
        var startingBalance = decimal.TryParse(StartingBalanceInput, out var b) ? b : CurrentBalance;
        var monthlyRate = annualRate / 12.0 / 100.0;
        var balance = (double)startingBalance;

        _projectionPoints.Clear();

        for (int i = 0; i <= ProjectionYears * 12; i++)
        {
            _projectionPoints.Add(new DateTimePoint(DateTime.Today.AddMonths(i), balance));
            balance = balance * (1 + monthlyRate) + (double)monthlySavings;
        }

        var finalBalance = (decimal)(_projectionPoints.Last().Value ?? 0);
        TotalSaved = monthlySavings * ProjectionYears * 12;
        TotalGains = finalBalance - startingBalance - TotalSaved;
    }

    private void UpdateSeries()
    {
        var series = new List<ISeries>();

        if (ShowHistorical && _historicalPoints.Count > 0)
            series.Add(new LineSeries<DateTimePoint>
            {
                Values = _historicalPoints.ToList(),
                Name = "Historical",
                GeometrySize = 0,
                Stroke = new SolidColorPaint(SKColors.SteelBlue) { StrokeThickness = 2 },
                Fill = null
            });

        if (ShowProjection && _projectionPoints.Count > 0)
            series.Add(new LineSeries<DateTimePoint>
            {
                Values = _projectionPoints.ToList(),
                Name = "Projection",
                GeometrySize = 0,
                Stroke = new SolidColorPaint(SKColors.MediumSeaGreen) { StrokeThickness = 2 },
                Fill = null
            });

        Series = series.ToArray();
    }
}
