﻿using BlazorBootstrap;
using Common;
using Common.ChartUtils;
using Common.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Statistik
{
    #region Constants
    private static readonly Modul ALLE_MODULE = new() { Id = 0, Name = "Alle Module" };
    private static readonly Question ALLE_QUESTIONS = new() { Id = 0, Text = "Alle Fragen", Type = 1 };
    #endregion

    #region Fields
    private readonly IList<Modul> module = new List<Modul>();
    private readonly IList<Question> questions = new List<Question>();
    private Modul? selectedModul = ALLE_MODULE;
    private Question? selectedQuestion = ALLE_QUESTIONS;

    // Chart
    private PieChart pieChart = new();
    private BarChart barChart = new();
    private PieChartOptions pieChartOptions = default!;
    private BarChartOptions barChartOptions = default!;
    private ChartData pieChartData = default!;
    private ChartData barChartData = default!;

    private bool bDisplayPieChart = true;
    private bool bDisplayBarChart;
    #endregion

    #region Properties
    [Inject]
    private IBasicLoader StandardLoader { get; set; } = null!;

    [Inject]
    private IFilterLoader FilteredLoader { get; set; } = null!;

    [Inject]
    private IBarChartLoader BarChartLoader { get; set; } = null!;

    [Inject]
    private IRepository<Modul> ModulRepository { get; set; } = null!;

    [Inject]
    private IRepository<Question> QuestionRepository { get; set; } = null!;
    #endregion

    #region Protecteds
    protected override void OnInitialized()
    {
        // Pie Chart
        PieChartOptionsGenerator pieOptions = new("Anzahl Antworten pro Frage");
        pieChartOptions = pieOptions.GetOptions();

        // Bar Chart
        BarChartOptionsGenerator barOptions = new("x", "Werte", "Anzahl Antworten");
        barChartOptions = barOptions.GetOptions();
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
        barChartData = await this.BarChartLoader.LoadData();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
        {
            UpdateChart();
            if(bDisplayPieChart) await pieChart.InitializeAsync(pieChartData, pieChartOptions);
            if(bDisplayBarChart) await barChart.InitializeAsync(barChartData, barChartOptions);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region Privates
    private async void UpdateChart()
    {
        // Standartdaten
        if(selectedModul is { Id: 0 } && selectedQuestion is { Id: 0 })
        {
            bDisplayBarChart = false;
            bDisplayPieChart = true;
            pieChartOptions.Plugins.Title!.Text = "Anzahl Antworten pro Frage";
            pieChartData = await this.StandardLoader.LoadData();
        }

        // Nur Modul bezogene Daten 
        if(selectedModul is not { Id: 0 } && selectedQuestion is { Id: 0 })
        {
            bDisplayBarChart = false;
            bDisplayPieChart = true;
            pieChartOptions.Plugins.Title!.Text = $"Auswertung zu {selectedModul?.Name}";
            pieChartData = await this.FilteredLoader.LoadData(selectedModul);
        }

        // Nur Fragen bezogene Daten
        if(selectedModul is { Id: 0 } && selectedQuestion is not { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.AuswahlFrage })
        {
            bDisplayBarChart = false;
            bDisplayPieChart = true;
            pieChartOptions.Plugins.Title!.Text = $"Auswertung zu der Frage {selectedQuestion?.Text}";
            pieChartData = await this.FilteredLoader.LoadData(selectedQuestion);
        }

        // Modul und Fragen bezogene Daten
        if(selectedModul is not { Id: 0 } && selectedQuestion is not { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.AuswahlFrage })
        {
            bDisplayBarChart = false;
            bDisplayPieChart = true;
            pieChartOptions.Plugins.Title!.Text = $"{selectedQuestion?.Text} aus {selectedModul?.Name}";
            pieChartData = await this.FilteredLoader.LoadData(selectedQuestion, selectedModul);
        }

        // Nur Fragenbezogene Daten für Zahlenbereiche
        if(selectedModul is { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.Zahlenbereich })
        {
            bDisplayBarChart = true;
            bDisplayPieChart = false;
            barChartOptions.Plugins.Title!.Text = $"Auswertung zu der Frage {selectedQuestion.Text}";
            barChartData = await this.BarChartLoader.LoadData(selectedQuestion);
        }

        // Modul und Fragen bezogene Daten mit Zahlenwert
        if(selectedModul is not { Id: 0 } && selectedQuestion is not { Id: 0 } && selectedQuestion is { Type: (int) QuestionType.Zahlenbereich })
        {
            bDisplayBarChart = true;
            bDisplayPieChart = false;
            barChartOptions.Plugins.Title!.Text = $"{selectedQuestion.Text} aus {selectedModul?.Name}";
            throw new NotImplementedException();
        }

        if(bDisplayPieChart) await pieChart.UpdateAsync(pieChartData, pieChartOptions);
        if(bDisplayBarChart) await barChart.UpdateAsync(barChartData, barChartOptions);
    }
    #endregion
}