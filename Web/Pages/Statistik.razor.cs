using BlazorBootstrap;
using Common;
using Common.ChartUtils;
using Common.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Statistik
{
    #region Constants
    private const string STD_TITLE = "Anzahl Antworten pro Frage";
    private static readonly Modul ALLE_MODULE = new() { Id = 0, Name = "Alle Module" };
    private static readonly Question ALLE_QUESTIONS = new() { Id = 0, Text = "Alle Fragen", Type = 1 };
    #endregion

    #region Fields
    private readonly IList<Modul> module = new List<Modul>();
    private readonly IList<Question> questions = new List<Question>();
    private Modul? selectedModul = ALLE_MODULE;
    private Question? selectedQuestion = ALLE_QUESTIONS;
    private readonly PieChartOptionsGenerator pieOptions = new(STD_TITLE);

    // Chart
    private PieChart pieChart = new();
    private BarChart barChart = new();
    private PieChartOptions pieChartOptions = default!;
    private BarChartOptions barChartOptions = default!;
    private ChartData pieChartData = default!;
    #endregion

    #region Properties
    [Inject]
    private IBasicLoader StandardLoader { get; set; } = null!;

    [Inject]
    private IFilterLoader FilteredLoader { get; set; } = null!;

    [Inject]
    private IRepository<Modul> ModulRepository { get; set; } = null!;

    [Inject]
    private IRepository<Question> QuestionRepository { get; set; } = null!;
    #endregion

    #region Protecteds
    protected override void OnInitialized()
    {
        // Pie Chart
        pieChartOptions = pieOptions.GetOptions();

        // Bar Chart
        barChartOptions = new BarChartOptions
                          {
                              Responsive = true,
                              Interaction = { Mode = InteractionMode.X },
                              IndexAxis = "x"
                          };

        barChartOptions.Scales.X!.Title!.Text = "Werte";
        barChartOptions.Scales.X.Title.Font!.Size = 24;
        barChartOptions.Scales.X.Title.Display = true;

        barChartOptions.Scales.Y!.Title!.Text = "Anzahl Antworten";
        barChartOptions.Scales.Y.Title.Font!.Size = 24;
        barChartOptions.Scales.Y.Title.Display = true;

        barChartOptions.Plugins.Legend.Display = false;
    }

    protected override async Task OnInitializedAsync()
    {
        module.Add(ALLE_MODULE);
        List<Modul> loadedModule = await this.ModulRepository.GetAllAsync();

        foreach(Modul m in loadedModule)
        {
            module.Add(m);
        }

        questions.Add(ALLE_QUESTIONS);
        List<Question> loadedQuestions = await this.QuestionRepository.GetAllAsync();

        foreach(Question q in loadedQuestions)
        {
            questions.Add(q);
        }

        pieChartData = await this.StandardLoader.LoadData();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
        {
            UpdateChart();
            await pieChart.InitializeAsync(pieChartData, pieChartOptions);

            // TODO - z.B IF zum nur eine Datei laden
            //await barChart.InitializeAsync(chartData, barChartOptions);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region Privates
    private async void UpdateChart()
    {
        if(selectedModul is { Id: 0 } && selectedQuestion is { Id: 0 })
        {
            pieChartOptions.Plugins.Title!.Text = STD_TITLE;
            pieChartData = await this.StandardLoader.LoadData();
        }

        if(selectedModul is not { Id: 0 } && selectedQuestion is { Id: 0 })
        {
            pieChartOptions.Plugins.Title!.Text = $"Auswertung zu {selectedModul?.Name}";
            pieChartData = await this.FilteredLoader.LoadData(selectedModul);
        }

        if(selectedModul is { Id: 0 } && selectedQuestion is not { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.AuswahlFrage })
        {
            pieChartOptions.Plugins.Title!.Text = $"Auswertung zu der Frage {selectedQuestion?.Text}";
            pieChartData = await this.FilteredLoader.LoadData(selectedQuestion);
        }

        if(selectedModul is not { Id: 0 } && selectedQuestion is not { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.AuswahlFrage })
        {
            pieChartOptions.Plugins.Title!.Text = $"{selectedQuestion?.Text} aus {selectedModul?.Name}";
            pieChartData = await this.FilteredLoader.LoadData(selectedQuestion, selectedModul);
        }

        if(selectedModul is { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.Zahlenbereich })
        {
            
        }
        //TODO BarCharts für Fragentypen einbauen --> über ENUM evaluieren

        await pieChart.UpdateAsync(pieChartData, pieChartOptions);
    }
    #endregion
}