using BlazorBootstrap;
using Common.ChartUtils;
using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Semester : ComponentBase
{
    #region Properties
    public BarChart BarChart { get; set; } = new();
    public BarChartOptions BarChartOptions { get; set; } = new();
    public ChartData BarChartData { get; set; } = new();
    #endregion

    #region Protecteds
    protected override void OnInitialized()
    {
        InitializeChartOptions();
    }

    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        return base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region Privates
    private void InitializeChartOptions()
    {
        this.BarChartOptions = new BarChartOptionsGenerator("x", "Fragen", "Anzahl Antworten").GetOptions();
    }

    private async Task LoadInitialDataAsync()
    {
        // TODO Allenfalls Filter Listen
        // TODO Semster Loader From BarChart Loader
    }
    #endregion
}