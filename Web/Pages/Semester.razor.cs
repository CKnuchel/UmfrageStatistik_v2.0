using BlazorBootstrap;
using Data.Context;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages;

public partial class Semester : ComponentBase
{
    #region Fields
    public List<string> test = new();
    public List<BlazorBootstrapChart> chartList = new();

    private PieChart testChart1 = new PieChart();
    private PieChart testChart2 = new PieChart();
    #endregion

    #region Properties
    [Inject]
    public IDbContextFactory<UmfrageContext> ContextFactory { get; set; } = default!;

    [Inject]
    private IBasicLoader BasicLoader { get; set; } = default!;
    #endregion

    #region Protecteds
    protected override void OnInitialized()
    {
        for(int i = 1; i <= 20; i++)
        {
            test.Add($"Ich bin die {i}. Zahl in der Liste.");
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await AddTestCharts();
    }
    #endregion

    #region Privates
    private async Task AddTestCharts()
    {
        // https://docs.blazorbootstrap.com/data-visualization/pie-chart Beispiel mit daten hinzufügen umdiese allenfalls später zu laden

        throw new NotImplementedException();
    }
    #endregion
}