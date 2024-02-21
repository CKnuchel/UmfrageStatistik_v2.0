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
    private List<int> availableYears = new List<int>();
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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await InitializeChartsAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
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
        this.BarChartOptions = new BarChartOptionsGenerator("y", "Anzahl Antworten", "Semester").GetOptions();
    }

    private async Task LoadInitialDataAsync()
    {
        // Modul Filter laden
        this.ModuleList.Add(DefaultModul);
        this.ModuleList.AddRange(await this.ModulRepository.GetAllAsync());

        // Anzeigen der Möglichen Jahre
        availableYears = await this.SemesterLoader.GetAvailableYears();
    }

    private async Task DetermineChartDataAsync()
    {
        // ohne Filter
        if(this.SelectedModul.Id == 0)
        {
            //TODO Loader erstellen
            //this.BarChartData = await this.SemesterLoader();
        }
        // Filterung nach Modul
        else
        {
            throw new NotImplementedException();
            //TODO Loader einbauen
            //this.BarChartData = await this.SemesterLoader();
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