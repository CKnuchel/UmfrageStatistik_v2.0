﻿using BlazorBootstrap;
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
    private List<Question> questions = new();
    private IList<int> years = new List<int>();
    private readonly List<string> labels = new();
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
        await CreateBasicLabels(responseRepository);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = await CreateBasicDataset(responseRepository)
               };
    }

    public async Task<ChartData> LoadDataByModul(int nModulId)
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);
        QuestionRepository questionRepository = new(context);

        questions = await questionRepository.GetAllAsync();

        years = await GetAvailableYears();

        await CreateLabelsByAvailableModule(responseRepository, nModulId);

        return new ChartData
               {
                   Labels = labels,
                   Datasets = await CreateDatasetByModulId(responseRepository, nModulId)
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
    private async Task CreateBasicLabels(ResponseRepository responseRepository)
    {
        if(years.Count == 0) throw new ArgumentException(nameof(years));

        foreach(int year in years)
        {
            if(await responseRepository.IsSemesterDataAvailable(year, 1)) labels.Add($"S1Y{year.ToString()[2..]}");
            if(await responseRepository.IsSemesterDataAvailable(year, 2)) labels.Add($"S2Y{year.ToString()[2..]}");
        }
    }

    private async Task CreateLabelsByAvailableModule(ResponseRepository responseRepository, int nModulId)
    {
        if(years.Count == 0) throw new ArgumentException(nameof(years));

        foreach(int year in years)
        {
            if(await responseRepository.IsSemesterDataAvailableByModuleId(year, 1, nModulId)) labels.Add($"S1Y{year.ToString()[2..]}");
            if(await responseRepository.IsSemesterDataAvailableByModuleId(year, 2, nModulId)) labels.Add($"S2Y{year.ToString()[2..]}");
        }
    }

    private async Task<List<IChartDataset>> CreateBasicDataset(ResponseRepository responseRepository)
    {
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
                if(await responseRepository.IsSemesterDataAvailable(year, 1)) dataList.Add(await responseRepository.GetResponseCountByQuestionIdAndSemesterAndYear(1, year, q.Id));
                if(await responseRepository.IsSemesterDataAvailable(year, 2)) dataList.Add(await responseRepository.GetResponseCountByQuestionIdAndSemesterAndYear(2, year, q.Id));
            }

            dataset.Data = dataList;

            datasets.Add(dataset);

            nColorIndex++;
        }

        return datasets;
    }

    private async Task<List<IChartDataset>> CreateDatasetByModulId(ResponseRepository responseRepository, int nModulId)
    {
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
                if(await responseRepository.IsSemesterDataAvailableByModuleId(year, 1, nModulId)) dataList.Add(await responseRepository.GetResponseCountByQuestionIdAndModulIdSemesterAndYear(1, year, q.Id, nModulId));
                if(await responseRepository.IsSemesterDataAvailableByModuleId(year, 2, nModulId)) dataList.Add(await responseRepository.GetResponseCountByQuestionIdAndModulIdSemesterAndYear(2, year, q.Id, nModulId));
            }

            dataset.Data = dataList;

            datasets.Add(dataset);

            nColorIndex++;
        }

        return datasets;
    }
    #endregion
}