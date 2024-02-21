using BlazorBootstrap;
using Common;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Logic.DataLoader
{
    public class BarChartLoader : IBarChartLoader
    {
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
        public async Task<ChartData> LoadData(Question? question = null, Modul modul = null!)
        {
            await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
            ResponseRepository responseRepository = new(context);

            // Auswahl des Ladeprozesses basierend auf den Uebergabeparametern
            List<IChartDataset> datasets = question == null
                ? await GetDatasetWithStandardData(responseRepository)
                : await GetDatasetByParameters(responseRepository, question, modul);

            return new ChartData
                   {
                       Labels = labels,
                       Datasets = datasets
                   };
        }
        #endregion

        #region Privates
        private static async Task<List<IChartDataset>> GetDatasetWithStandardData(ResponseRepository repository)
        {
            return await GenericDatasetFetch(repository);
        }

        private static async Task<List<IChartDataset>> GetDatasetByParameters(ResponseRepository repository, Question question, Modul modul)
        {
            return await GenericDatasetFetch(repository, question, modul);
        }

        private static async Task<List<IChartDataset>> GenericDatasetFetch(ResponseRepository repository, Question? question = null, Modul? modul = null)
        {
            List<IChartDataset> datasets = new();
            List<double> answerCount = new();

            for(int i = 1; i <= 10; i++)
            {
                int nCount;
                if(question == null)
                {
                    nCount = await repository.GetResponseCountByQuestionTypeAndValue(i, (int) QuestionType.Zahlenbereich);
                }
                else if(modul == null)
                {
                    nCount = await repository.GetResponseCountByQuestionIdAndValue(question.Id, i);
                }
                else
                {
                    nCount = await repository.GetResponseCountByQuesionIdAndModulIdAndValue(modul.Id, question.Id, i);
                }

                answerCount.Add(nCount);
            }

            datasets.Add(new BarChartDataset
                         {
                             Label = "Erhaltene Antworten",
                             Data = answerCount,
                             BackgroundColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                             BorderColor = new List<string> { ColorGenerator.CategoricalTwentyColors()[1] },
                             BorderWidth = new List<double> { 0 }
                         });

            return datasets;
        }
        #endregion
    }
}