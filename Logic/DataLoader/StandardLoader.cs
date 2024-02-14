using BlazorBootstrap;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Logic.DataLoader
{
    public class StandardLoader : IBasicLoader
    {
        #region Fields
        private IList<Question> questions = new List<Question>();
        private readonly IDbContextFactory<UmfrageContext> _contextFactory;
        #endregion

        #region Constructors
        public StandardLoader(IDbContextFactory<UmfrageContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        #endregion

        #region Publics
        public async Task<ChartData> LoadData()
        {
            await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
            QuestionRepository questionRepository = new QuestionRepository(context);
            ResponseRepository responseRepository = new ResponseRepository(context);

            questions = await questionRepository.GetAllAsync();

            List<IChartDataset> datasets = await GetAnswerCountByQuestion(responseRepository);

            return new ChartData
                   {
                       Labels = questions.Select(q => q.Text).ToList(),
                       Datasets = datasets
                   };
        }
        #endregion

        #region Privates
        private async Task<List<IChartDataset>> GetAnswerCountByQuestion(ResponseRepository responseRepository)
        {
            List<double> answerCount = new();
            List<IChartDataset> datasets = new();

            foreach(Question question in questions)
            {
                int count = await responseRepository.GetResponseCountByQuestionIdAsync(question.Id);
                answerCount.Add(count);
            }

            datasets.Add(new PieChartDataset
                         {
                             Label = "Erhaltene Antworten",
                             Data = answerCount,
                             BackgroundColor = ColorGenerator.CategoricalTwentyColors().ToList()
                         });

            return datasets;
        }
        #endregion
    }
}