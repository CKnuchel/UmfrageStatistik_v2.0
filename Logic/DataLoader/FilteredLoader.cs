using BlazorBootstrap;
using Common.ChartUtils;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Logic.DataLoader
{
    public class FilteredLoader : IFilterLoader
    {
        #region Fields
        private readonly IDbContextFactory<UmfrageContext> _contextFactory;
        private IList<Question> questions = new List<Question>();
        private IList<Answer> answers = new List<Answer>();
        #endregion

        #region Constructors
        public FilteredLoader(IDbContextFactory<UmfrageContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        #endregion

        #region Publics
        public async Task<ChartData> LoadData(Modul? modul)
        {
            if(modul == null) throw new ArgumentNullException(nameof(modul));

            await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
            QuestionRepository questionRepository = new(context);
            ResponseRepository responseRepository = new(context);

            questions = await questionRepository.GetAllAsync();

            List<IChartDataset> datasets = await GetAnswerCountByQuestionAndModulId(modul.Id, responseRepository);

            return new ChartData
                   {
                       Labels = questions.Select(q => q.Text).ToList(),
                       Datasets = datasets
                   };
        }

        public async Task<ChartData> LoadData(Question? question)
        {
            if(question == null) throw new ArgumentNullException(nameof(question));

            await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
            AnswerRepository answerRepository = new(context);
            ResponseRepository responseRepository = new(context);

            answers = await answerRepository.GetByQuestionId(question.Id);

            List<IChartDataset> datasets = await GetAnswerCountByAnswerId(responseRepository);

            return new ChartData
                   {
                       Labels = answers.Select(q => q.Text.Split('(')[0].Trim()).ToList(),
                       Datasets = datasets
                   };
        }

        public async Task<ChartData> LoadData(Question? question, Modul? modul)
        {
            if(question == null) throw new ArgumentNullException(nameof(question));
            if(modul == null) throw new ArgumentNullException(nameof(modul));

            await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
            QuestionRepository questRepository = new(context);
            AnswerRepository answerRepository = new(context);
            ResponseRepository responseRepository = new(context);

            questions = await questRepository.GetAllAsync();
            answers = await answerRepository.GetByQuestionId(question.Id);

            List<IChartDataset> datasets = await GetAnswerCountByAnswerIdAndModulIdAsync(modul.Id, responseRepository);

            return new ChartData
                   {
                       Labels = answers.Select(q => q.Text.Split('(')[0].Trim()).ToList(),
                       Datasets = datasets
                   };
        }
        #endregion

        #region Privates
        private async Task<List<IChartDataset>> GetAnswerCountByQuestionAndModulId(int modulId, ResponseRepository responseRepository)
        {
            List<double> answerCount = new();
            List<IChartDataset> datasets = new();

            foreach(Question question in questions)
            {
                int count = await responseRepository.GetResponseCountByModuleIdAndQuestionIdAsync(modulId, question.Id);
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

        private async Task<List<IChartDataset>> GetAnswerCountByAnswerId(ResponseRepository responseRepository)
        {
            List<double> answerCount = new();
            List<IChartDataset> dataset = new();

            foreach(Answer answer in answers)
            {
                int count = await responseRepository.GetResponseCountByAnswerIdAsync(answer.Id);
                answerCount.Add(count);
            }

            dataset.Add(new PieChartDataset
                        {
                            Label = "Anzahl Antworten",
                            Data = answerCount,
                            BackgroundColor = ColorGenerator.CategoricalTwentyColors().ToList()
                        });

            return dataset;
        }

        private async Task<List<IChartDataset>> GetAnswerCountByAnswerIdAndModulIdAsync(int modulId, ResponseRepository responseRepository)
        {
            List<double> answerCount = new();
            List<IChartDataset> dataset = new();

            foreach(Answer answer in answers)
            {
                int count = await responseRepository.GetResponseCountByAnswerIdAndModulId(modulId, answer.Id);
                answerCount.Add(count);
            }

            dataset.Add(new PieChartDataset
                        {
                            Label = "Anzahl Antworten",
                            Data = answerCount,
                            BackgroundColor = ColorGenerator.CategoricalTwentyColors().ToList()
                        });

            return dataset;
        }
        #endregion
    }
}