using BlazorBootstrap;
using Common.ChartUtils;
using Common.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Semester : ComponentBase
{
    #region Constants
    private static readonly Modul DefaultModul = new() { Id = 0, Name = "Alle Module" };
    #endregion

    #region Fields
    private bool IsBarChartInitialized;
    #endregion

    #region Properties
    public Modul SelectedModul { get; set; } = DefaultModul;
    public List<Modul> ModuleList { get; set; } = new();
    public BarChart BarChart { get; set; } = new();
    public BarChartOptions BarChartOptions { get; set; } = new();
    public ChartData BarChartData { get; set; } = new();

    [Inject]
    public IRepository<Modul> ModulRepository { get; set; } = default!;

    [Inject]
    private ISemesterLoader SemesterLoader { get; set; } = default!;
    #endregion

    #region Publics
    public async Task SetSelectedModul(Modul modul)
    {
        this.SelectedModul = modul;
        await LoadDataBasedOnSelectionAsync();
    }
    #endregion

    #region Protecteds
    protected override void OnInitialized()
    {
        InitializeChartOptions();
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadInitialDataAsync();
    }

    protected override async Task OnAfterRenderAsync(bool bFirstRender)
    {
        if(bFirstRender)
        {
            await InitializeChartsAsync();
        }

        await base.OnAfterRenderAsync(bFirstRender);
    }
    #endregion

    #region Privates
    private async Task InitializeChartsAsync()
    {
        await DetermineChartDataAsync();
        await UpdateDisplayedChartsAsync();
    }

    private async Task LoadDataBasedOnSelectionAsync()
    {
        await DetermineChartDataAsync();
        await UpdateDisplayedChartsAsync();
        StateHasChanged();
    }

    private void InitializeChartOptions()
    {
        this.BarChartOptions = new BarChartOptionsGenerator("y", "Anzahl Antworten", string.Empty, true).GetOptions();

        // Individuelle Anpassungen
        this.BarChartOptions.Plugins.Title!.Text = "Semesterentwicklung";
        this.BarChartOptions.Plugins.Title!.Font!.Size = 24;
        this.BarChartOptions.Plugins.Title.Display = true;
        this.BarChartOptions.Plugins.Legend.Display = true;
        this.BarChartOptions.Plugins.Legend.Align = "left";
        this.BarChartOptions.Plugins.Legend.Position = "bottom";
    }

    private async Task LoadInitialDataAsync()
    {
        // Modul Filter laden
        this.ModuleList.Add(DefaultModul);
        this.ModuleList.AddRange(await this.ModulRepository.GetAllAsync());
    }

    private async Task DetermineChartDataAsync()
    {
        this.BarChartData.Labels?.Clear();
        this.BarChartData.Datasets?.Clear();

        // ohne Filter
        if(this.SelectedModul.Id == 0)
        {
            this.BarChartData = await this.SemesterLoader.LoadData();
        }
        // Filterung nach Modul
        else
        {
            this.BarChartData = await this.SemesterLoader.LoadDataByModul(this.SelectedModul.Id);
        }
    }

    private async Task UpdateDisplayedChartsAsync()
    {
        if(!IsBarChartInitialized)
        {
            await this.BarChart.InitializeAsync(this.BarChartData, this.BarChartOptions);
            IsBarChartInitialized = true;
        }
        else
        {
            await this.BarChart.UpdateAsync(this.BarChartData, this.BarChartOptions);
        }
    }
    #endregion
}