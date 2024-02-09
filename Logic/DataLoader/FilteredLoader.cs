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
        private IList<Answer> answers = new List<Answer>();
        private readonly QuestionRepository _questionRepository;
        private readonly ResponseRepository _responseRepository;
        private readonly AnswerRepository _answerRepository;
        #endregion

        #region Constructors
        public FilteredLoader(UmfrageContext context)
        {
            _questionRepository = new QuestionRepository(context);
            _responseRepository = new ResponseRepository(context);
            _answerRepository = new AnswerRepository(context);
        }
        #endregion

        #region Publics
        public async Task<ChartData> LoadData(Modul? modul)
        {
            if(modul == null) throw new ArgumentNullException(nameof(modul));

            questions = await _questionRepository.GetAllAsync();

            List<IChartDataset> datasets = await GetAnswerCountByQuestionAndModulId(modul.Id);

            return new ChartData
                   {
                       Labels = questions.Select(q => q.Text).ToList(),
                       Datasets = datasets
                   };
        }

        public async Task<ChartData> LoadData(Question? question)
        {
            if(question == null) throw new ArgumentNullException(nameof(question));

            answers = await _answerRepository.GetByQuestionId(question.Id);

            List<IChartDataset> datasets = await GetAnswerCountByAnswerId();

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

            questions = await _questionRepository.GetAllAsync();
            answers = await _answerRepository.GetByQuestionId(question.Id);

            List<IChartDataset> datasets = await GetAnswerCountByAnswerIdAndModulIdAsync(modul.Id);

            return new ChartData
                   {
                       Labels = answers.Select(q => q.Text.Split('(')[0].Trim()).ToList(),
                       Datasets = datasets
                   };
        }
        #endregion

        #region Privates
        private async Task<List<IChartDataset>> GetAnswerCountByQuestionAndModulId(int modulId)
        {
            List<double> answerCount = new();
            List<IChartDataset> datasets = new();

            foreach(Question question in questions)
            {
                int count = await _responseRepository.GetResponseCountByModuleIdAndQuestionIdAsync(modulId, question.Id);
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

        private async Task<List<IChartDataset>> GetAnswerCountByAnswerId()
        {
            List<double> answerCount = new();
            List<IChartDataset> dataset = new();

            foreach(Answer answer in answers)
            {
                int count = await _responseRepository.GetResponseCountByAnswerIdAsync(answer.Id);
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

        private async Task<List<IChartDataset>> GetAnswerCountByAnswerIdAndModulIdAsync(int modulId)
        {
            List<double> answerCount = new();
            List<IChartDataset> dataset = new();

            foreach(Answer answer in answers)
            {
                int count = await _responseRepository.GetResponseCountByAnswerIdAndModulId(modulId, answer.Id);
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