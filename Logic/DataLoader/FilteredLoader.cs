using BlazorBootstrap;
using Common;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;

namespace Logic.DataLoader
{
    public class FilteredLoader : IFilterLoader
    {
        #region Fields
        private IList<Question> questions = new List<Question>();
        private readonly QuestionRepository _questionRepository;
        private readonly ResponseRepository _responseRepository;
        #endregion

        #region Constructors
        public FilteredLoader(UmfrageContext context)
        {
            _questionRepository = new QuestionRepository(context);
            _responseRepository = new ResponseRepository(context);
        }
        #endregion

        #region Publics
        public async Task<ChartData> LoadData(Modul? modul)
        {
            if(modul == null) return null!;

            questions = await _questionRepository.GetAllAsync();

            List<IChartDataset> datasets = await GetAnswerCountByQuestionAndModulId(modul.Id);

            return new ChartData
                   {
                       Labels = questions.Select(q => q.Text).ToList(),
                       Datasets = datasets
                   };
        }

        private async Task<List<IChartDataset>> GetAnswerCountByQuestionAndModulId(int modulId)
        {
            List<double> answerCount = new();
            List<IChartDataset> datasets = new();

            foreach(Question question in questions)
            {
                int count = await _responseRepository.GetAnswerCountByModuleIdAndQuestionIdAsync(modulId, question.Id);
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