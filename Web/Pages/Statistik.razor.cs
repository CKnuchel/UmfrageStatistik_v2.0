using BlazorBootstrap;
using Common.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Statistik
{
    #region Constants
    private const string STD_TITLE = "Anzahl Antworten pro Frage";
    private static readonly Modul ALLE_MODULE = new() { Id = 0, Name = "Alle anzeigen" };
    #endregion

    #region Fields
    private readonly IList<Modul> module = new List<Modul>();
    private Modul? selectedModul = ALLE_MODULE;

    // Chart
    private PieChart pieChart = new();
    private PieChartOptions pieChartOptions = default!;
    private ChartData chartData = default!;
    #endregion

    #region Properties
    [Inject]
    private IBasicLoader StandardLoader { get; set; } = null!;

    [Inject]
    private IFilterLoader FilteredLoader { get; set; } = null!;

    [Inject]
    private IRepository<Modul> ModulRepository { get; set; } = null!;
    #endregion

    #region Protecteds
    protected override void OnInitialized()
    {
        pieChartOptions = new PieChartOptions
                          {
                              Responsive = true,
                              Plugins = new PieChartPlugins
                                        {
                                            Title = new ChartPluginsTitle
                                                    {
                                                        Display = true,
                                                        Text = STD_TITLE
                                                    }
                                        }
                          };
        pieChartOptions.Plugins.Title.Font!.Size = 24;
        pieChartOptions.Plugins.Legend.Position = "bottom";
    }

    protected override async Task OnInitializedAsync()
    {
        module.Add(ALLE_MODULE);
        List<Modul> loadedModule = await this.ModulRepository.GetAllAsync();

        foreach(Modul m in loadedModule)
        {
            module.Add(m);
        }

        chartData = await this.StandardLoader.LoadData(); // Laden der Standard Daten
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
        {
            UpdateChart();
            await pieChart.InitializeAsync(chartData, pieChartOptions);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region Privates
    private async void UpdateChart()
    {
        if(selectedModul is { Id: 0 })
        {
            pieChartOptions.Plugins.Title!.Text = STD_TITLE;
            chartData = await this.StandardLoader.LoadData();
            await pieChart.UpdateAsync(chartData, pieChartOptions);
        }
        else
        {
            pieChartOptions.Plugins.Title!.Text = $"Auswertung {selectedModul?.Name}";
            chartData = await this.FilteredLoader.LoadData(selectedModul);
            await pieChart.UpdateAsync(chartData, pieChartOptions);
        }
    }
    #endregion
}