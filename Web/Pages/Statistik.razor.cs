using BlazorBootstrap;
using Common.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Statistik
{
    #region Constants
    private const string STD_TITLE = "Anzahl Antworten pro Frage";
    #endregion

    #region Fields
    private IList<Modul> module = new List<Modul>();
    private Modul selectedModul = new() { Name = "Modul wählen" };

    // Chart
    private PieChart pieChart = new();
    private PieChartOptions pieChartOptions = default!;
    private ChartData chartData = default!;
    #endregion

    #region Properties
    [Inject]
    private ILoader StandardLoader { get; set; } = null!;

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
        module = await this.ModulRepository.GetAllAsync(); // Laden der Module für das Dropdown
        chartData = await this.StandardLoader.LoadData(); // Laden der Standard Daten
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
        {
            await pieChart.InitializeAsync(chartData, pieChartOptions);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region Privates
    private async void UpdateChart()
    {
        pieChartOptions.Plugins.Title!.Text = $"Auswertung {selectedModul.Name}";
        await pieChart.UpdateAsync(chartData, pieChartOptions); // Updaten des Chart und der Labels
    }
    #endregion
}