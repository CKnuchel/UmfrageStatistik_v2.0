using BlazorBootstrap;
using Common;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Logic.DataLoader;

public class BarChartLoader : IBarChartLoader
{
    #region Constants
    private const string LABEL_TOOLTIP = "Erhaltene Antworten";
    #endregion

    #region Fields
    private readonly IDbContextFactory<UmfrageContext> _contextFactory;
    private readonly List<string> labels = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
    #endregion

    #region Constructors
    public BarChartLoader(IDbContextFactory<UmfrageContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    #endregion

    #region Publics
    public async Task<ChartData> LoadData()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = await GetDatasetWithStandartData(responseRepository)
               };
    }

    public async Task<ChartData> LoadData(Question question)
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = await GetDatasetByQuestionId(question.Id, responseRepository)
               };
    }

    public async Task<ChartData> LoadData(Question question, Modul modul)
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = await GetDatasetByQuestionAndModul(question, modul, responseRepository)
               };
    }

    public async Task<ChartData> LoadSemesterData()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = null
               };

        // TODO Semester Daten laden
        throw new NotImplementedException();
    }

    public async Task<ChartData> LoadSemesterDataByModul(Modul modul)
    {
        if(modul == null) throw new ArgumentNullException(nameof(modul));

        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = null
               };

        // TODO Semester Daten mit Modul Filter laden
        throw new NotImplementedException();
    }
    #endregion

    #region Privates
    private async Task<List<IChartDataset>> GetDatasetWithStandartData(ResponseRepository responseRepository)
    {
        List<double> answerCount = new();
        List<IChartDataset> datasets = new();

        for(int i = 1; i <= 10; i++)
        {
            int nCount = await responseRepository.GetResponseCountByQuestionTypeAndValue(i, (int) QuestionType.Zahlenbereich);
            answerCount.Add(nCount);
        }

        datasets.Add(new BarChartDataset
                     {
                         Label = LABEL_TOOLTIP,
                         Data = answerCount,
                         BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                         BorderColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                         BorderWidth = new List<double> { 0 }
                     }
                    );

        return datasets;
    }

    private async Task<List<IChartDataset>> GetDatasetByQuestionId(int questionId, ResponseRepository responseRepository)
    {
        List<double> answerCount = new();
        List<IChartDataset> datasets = new();

        for(int i = 1; i <= 10; i++)
        {
            int nCount = await responseRepository.GetResponseCountByQuestionIdAndValue(questionId, i);
            answerCount.Add(nCount);
        }

        datasets.Add(new BarChartDataset
                     {
                         Label = LABEL_TOOLTIP,
                         Data = answerCount,
                         BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                         BorderColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                         BorderWidth = new List<double> { 0 }
                     }
                    );

        return datasets;
    }

    private async Task<List<IChartDataset>> GetDatasetByQuestionAndModul(Question question, Modul modul, ResponseRepository responseRepository)
    {
        List<double> answerCount = new();
        List<IChartDataset> datasets = new();

        for(int i = 1; i <= 10; i++)
        {
            int nCount = await responseRepository.GetResponseCountByQuesionIdAndModulIdAndValue(modul.Id, question.Id, i);
            answerCount.Add(nCount);
        }

        datasets.Add(new BarChartDataset
                     {
                         Label = LABEL_TOOLTIP,
                         Data = answerCount,
                         BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                         BorderColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                         BorderWidth = new List<double> { 0 }
                     }
                    );

        return datasets;
    }
    #endregion
}