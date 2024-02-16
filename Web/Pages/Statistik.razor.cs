using BlazorBootstrap;
using Common;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages;

public partial class Statistik : ComponentBase
{
    #region Constants
    private static readonly Modul DefaultModul = new() { Id = 0, Name = "Alle Module" };
    private static readonly Question DefaultQuestion = new() { Id = 0, Text = "Alle Fragen", Type = 1 };
    #endregion

    #region Fields
    private bool IsPieChartInitialized;
    private bool IsBarChartInitialized;
    #endregion

    #region Properties
    public Modul SelectedModul { get; set; } = DefaultModul;
    public Question SelectedQuestion { get; set; } = DefaultQuestion;
    public List<Modul> ModuleList { get; set; } = new();
    public List<Question> QuestionList { get; set; } = new();
    public PieChart PieChart { get; set; } = new();
    public BarChart BarChart { get; set; } = new();
    public PieChartOptions PieChartOptions { get; set; } = new();
    public BarChartOptions BarChartOptions { get; set; } = new();
    public ChartData PieChartData { get; set; } = new();
    public ChartData BarChartData { get; set; } = new();
    public bool DisplayPieChart { get; set; } = true;
    public bool DisplayBarChart { get; set; }

    public IList<BlazorBootstrapChart>? ChartListeTest { get; set; }

    [Inject]
    public IBasicLoader BasicLoader { get; set; } = default!;

    [Inject]
    public IFilterLoader FilterLoader { get; set; } = default!;

    [Inject]
    public IBarChartLoader BarChartLoader { get; set; } = default!;

    [Inject]
    public IRepository<Modul> ModulRepository { get; set; } = default!;

    [Inject]
    public IRepository<Question> QuestionRepository { get; set; } = default!;

    [Inject]
    public IDbContextFactory<UmfrageContext> ContextFactory { get; set; } = default!;
    #endregion

    #region Publics
    public async Task SetSelectedModul(Modul modul)
    {
        this.SelectedModul = modul;
        await LoadDataBasedOnSelectionAsync();
    }

    public async Task SetSelectedQuestion(Question question)
    {
        this.SelectedQuestion = question;
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
    private async Task LoadDataBasedOnSelectionAsync()
    {
        await DetermineChartDataAsync();
        await UpdateDisplayedChartsAsync();
        StateHasChanged();
    }

    private void InitializeChartOptions()
    {
        this.PieChartOptions = new PieChartOptionsGenerator("Anzahl Antworten pro Frage").GetOptions();
        this.BarChartOptions = new BarChartOptionsGenerator("x", "Werte", "Anzahl Antworten").GetOptions();
    }

    private async Task LoadInitialDataAsync()
    {
        this.ModuleList.Add(DefaultModul);
        this.QuestionList.Add(DefaultQuestion);

        List<Modul> loadedModulesTask = await this.ModulRepository.GetAllAsync();
        List<Question> loadedQuestionsTask = await this.QuestionRepository.GetAllAsync();

        this.ModuleList.AddRange(loadedModulesTask);
        this.QuestionList.AddRange(loadedQuestionsTask);
    }

    private async Task InitializeChartsAsync()
    {
        await DetermineChartDataAsync();
        await UpdateDisplayedChartsAsync();
        IsPieChartInitialized = this.DisplayPieChart;
        IsBarChartInitialized = this.DisplayBarChart;
    }

    private async Task DetermineChartDataAsync()
    {
        // ohne Filter (Typ 1 und 2) i.O
        if(this.SelectedModul.Id == 0 && this.SelectedQuestion.Id == 0)
        {
            this.PieChartData = await this.BasicLoader.LoadData();
            this.DisplayPieChart = true;
            this.DisplayBarChart = false;
        }
        // Filterung nach Modul (Typ 1 und 2) i.O
        else if(this.SelectedModul.Id != 0 && this.SelectedQuestion is { Id: 0, Type: (int) QuestionType.AuswahlFrage })
        {
            this.PieChartData = await this.FilterLoader.LoadData(this.SelectedModul);
            this.DisplayPieChart = true;
            this.DisplayBarChart = false;
        }
        // Filterung nach Frage ( Typ 1) i.O
        else if(this.SelectedModul.Id == 0 && this.SelectedQuestion.Id != 0 && this.SelectedQuestion.Type == (int) QuestionType.AuswahlFrage)
        {
            this.PieChartData = await this.FilterLoader.LoadData(this.SelectedQuestion);
            this.DisplayPieChart = true;
            this.DisplayBarChart = false;
        }
        // Filterung nach Modul und Frage (Typ 1)
        else if(this.SelectedQuestion.Type == (int) QuestionType.AuswahlFrage && this.SelectedQuestion.Id != 0 && this.SelectedModul.Id != 0)
        {
            this.PieChartData = await this.FilterLoader.LoadData(this.SelectedQuestion, this.SelectedModul);
            this.DisplayPieChart = true;
            this.DisplayBarChart = false;
        }
        // Filterung nach Frage und Modul (Typ 2)
        else if(this.SelectedQuestion.Type == (int) QuestionType.Zahlenbereich && this.SelectedQuestion is not { Id: 0 } && this.SelectedModul is not { Id: 0 })
        {
            this.BarChartData = await this.BarChartLoader.LoadData(this.SelectedQuestion, this.SelectedModul);
            this.DisplayPieChart = false;
            this.DisplayBarChart = true;
        }
        // Filterung nach Frage (Typ 2)
        else if(this.SelectedQuestion.Type == (int) QuestionType.Zahlenbereich && this.SelectedQuestion is not { Id: 0 })
        {
            this.BarChartData = await this.BarChartLoader.LoadData(this.SelectedQuestion);
            this.DisplayPieChart = false;
            this.DisplayBarChart = true;
        }
    }

    private async Task UpdateDisplayedChartsAsync()
    {
        StateHasChanged(); // Stellt sicher, dass zuvor das Objekt im DOM geladen wurde

        if(this.DisplayPieChart)
        {
            if(!IsPieChartInitialized)
            {
                await this.PieChart.InitializeAsync(this.PieChartData, this.PieChartOptions);
                IsPieChartInitialized = true;
            }
            else
            {
                await this.PieChart.UpdateAsync(this.PieChartData, this.PieChartOptions);
            }

            if(IsBarChartInitialized)
            {
                await this.BarChart.DisposeAsync();
                IsBarChartInitialized = false;
            }
        }
        else if(this.DisplayBarChart)
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

            if(IsPieChartInitialized)
            {
                await this.PieChart.DisposeAsync();
                IsPieChartInitialized = false;
            }
        }
    }
    #endregion
}