using BlazorBootstrap;
using Common;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;

namespace Logic.DataLoader;

public class BarChartLoader : IBarChartLoader
{
    #region Fields
    private readonly ResponseRepository _responseRepository;
    #endregion

    #region Constructors
    public BarChartLoader(UmfrageContext context)
    {
        _responseRepository = new ResponseRepository(context);
    }
    #endregion

    #region Publics
    public async Task<ChartData> LoadData(Question question)
    {
        return new ChartData
               {
                   Labels = FillLabelsFrom1To10(),
                   Datasets = await GetDatasetByQuestionId(question.Id)
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

    private async Task<List<IChartDataset>> GetDatasetByQuestionId(int questionId)
    {
        List<double> answerCount = new();
        List<IChartDataset> datasets = new();

        for(int i = 1; i <= 10; i++)
        {
            int nCount = await _responseRepository.GetResponseCountByQuestionIdAndValue(questionId, (int) QuestionType.Zahlenbereich);
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