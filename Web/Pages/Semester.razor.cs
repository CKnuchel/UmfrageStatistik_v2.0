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

    #region Properties
    public Modul SelectedModul { get; set; } = DefaultModul;
    public List<Modul> ModuleList { get; set; } = new();
    public BarChart BarChart { get; set; } = new();
    public BarChartOptions BarChartOptions { get; set; } = new();
    public ChartData BarChartData { get; set; } = new();

    [Inject]
    public IRepository<Modul> ModulRepository { get; set; } = default!;
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
    private async Task LoadDataBasedOnSelectionAsync()
    {
        //await DetermineChartDataAsync();
        //await UpdateDisplayedChartsAsync();
        StateHasChanged();
    }

    private void InitializeChartOptions()
    {
        this.BarChartOptions = new BarChartOptionsGenerator("x", "Fragen", "Anzahl Antworten").GetOptions();
    }

    private async Task LoadInitialDataAsync()
    {
        // Modul Filter laden
        this.ModuleList.Add(DefaultModul);
        List<Modul> loadedModulesTask = await this.ModulRepository.GetAllAsync();
        this.ModuleList.AddRange(loadedModulesTask);
        // TODO Semster Loader From BarChart Loader
    }
    #endregion
}