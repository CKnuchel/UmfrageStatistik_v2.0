using BlazorBootstrap;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Logic.DataLoader;

public class SemesterLoader : ISemesterLoader
{
    #region Fields
    private readonly IDbContextFactory<UmfrageContext> _contextFactory;
    private List<Question> questions = new List<Question>();
    private IList<int> years = new List<int>();
    private readonly List<string> labels = new List<string>();
    #endregion

    #region Constructors
    public SemesterLoader(IDbContextFactory<UmfrageContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    #endregion

    #region Publics
    public async Task<ChartData> LoadData()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);
        QuestionRepository questionRepository = new(context);

        // Mögliche Fragen auslesen
        questions = await questionRepository.GetAllAsync();

        //Jahre auslesen
        years = await GetAvailableYears();

        // Chart Labels füllen
        CreateLabels();

        return new ChartData
               {
                   Labels = labels,
                   Datasets = await CreateBasicDataset(responseRepository)
               };
    }

    public async Task<List<int>> GetAvailableYears()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return await responseRepository.GetAvailableYearsFromResponses();
    }
    #endregion

    #region Privates
    private void CreateLabels()
    {
        if(years.Count == 0) throw new ArgumentException(nameof(years));

        foreach(int year in years)
        {
            labels.Add($"S1Y{year.ToString().Substring(2)}");
            labels.Add($"S2Y{year.ToString().Substring(2)}");
        }
    }

    private async Task<List<IChartDataset>> CreateBasicDataset(ResponseRepository responseRepository)
    {
        // TODO Daten werden Fehlerhaft geladen

        List<IChartDataset> datasets = new();
        int nColorIndex = 0;

        foreach(Question q in questions)
        {
            List<double> dataList = new();

            BarChartDataset dataset = new()
                                      {
                                          Label = q.Text,
                                          BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[nColorIndex] },
                                          BorderColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[nColorIndex] },
                                          BorderWidth = new List<double> { 0 }
                                      };

            foreach(int year in years)
            {
                dataList.Add(await responseRepository.GetResponseCountByQuestionIdAndSemesterAndYear(1, year, q.Id));
                dataList.Add(await responseRepository.GetResponseCountByQuestionIdAndSemesterAndYear(2, year, q.Id));
            }

            dataset.Data = dataList;

            datasets.Add(dataset);

            nColorIndex++;
        }

        return datasets;
    }
    #endregion
}