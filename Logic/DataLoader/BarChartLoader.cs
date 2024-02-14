﻿using BlazorBootstrap;
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
    #region Fields
    private readonly IDbContextFactory<UmfrageContext> _contextFactory;
    #endregion

    #region Constructors
    public BarChartLoader(IDbContextFactory<UmfrageContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    #endregion

    #region Publics
    public async Task<ChartData> LoadData(Question question)
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = FillLabelsFrom1To10(),
                   Datasets = await GetDatasetByQuestionId(question.Id, responseRepository)
               };
    }

    public async Task<ChartData> LoadData()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return new ChartData
               {
                   Labels = FillLabelsFrom1To10(),
                   Datasets = await GetDatasetByQuestionType((int) QuestionType.Zahlenbereich, responseRepository)
               };
    }
    #endregion

    #region Privates
    private List<string> FillLabelsFrom1To10()
    {
        List<string> labels = new List<string>();

        for(int i = 1; i <= 10; i++)
        {
            labels.Add(i.ToString());
        }

        return labels;
    }

    private async Task<List<IChartDataset>> GetDatasetByQuestionId(int questionId, ResponseRepository responseRepository)
    {
        List<double> answerCount = new();
        List<IChartDataset> datasets = new();

        for(int i = 1; i <= 10; i++)
        {
            int nCount = await responseRepository.GetResponseCountByQuestionIdAndValue(questionId, (int) QuestionType.Zahlenbereich);
            answerCount.Add(nCount);
        }

        datasets.Add(new BarChartDataset
                     {
                         Data = answerCount,
                         BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[0] },
                         BorderColor = new List<string> { "FFFFFF" },
                         BorderWidth = new List<double> { 0 }
                     }
                    );

        return datasets;
    }

    private async Task<List<IChartDataset>> GetDatasetByQuestionType(int nType, ResponseRepository responseRepository)
    {
        List<double> answerCount = new();
        List<IChartDataset> datasets = new();

        for(int i = 1; i <= 10; i++)
        {
            int nCount = await responseRepository.GetResponseCountByQuestionTypeAndValue(i, nType);
            answerCount.Add(nCount);
        }

        datasets.Add(new BarChartDataset
                     {
                         Data = answerCount,
                         BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[0] },
                         BorderColor = new List<string> { "FFFFFF" },
                         BorderWidth = new List<double> { 0 }
                     }
                    );

        return datasets;
    }
    #endregion
}