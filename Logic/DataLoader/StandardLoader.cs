using BlazorBootstrap;
using Common;
using Common.Models;
using Data.Context;
using Logic.Repository;

namespace Logic.DataLoader
{
    public class StandardLoader
    {
        #region Fields
        private readonly UmfrageContext _context;
        private IList<Question> questions = new List<Question>();
        private QuestionRepository _questionRepository;
        private ResponseRepository _responseRepository;
        #endregion

        #region Constructors
        public StandardLoader(UmfrageContext context)
        {
            _context = context;
            _questionRepository = new QuestionRepository(_context);
            _responseRepository = new ResponseRepository(_context);
        }
        #endregion

        #region Publics
        public async Task<ChartData> LoadData()
        {
            await LoadQuestions();

            var datasets = await GetAnswerCountByQuestion();

            return new ChartData
            {
                Labels = LoadLabels(),
                Datasets = datasets
            };
        }

        public async Task<string> GetTestString()
        {
            await LoadQuestions();

            return questions.Count > 0 ? $"First Row in Questions. Id: {questions[0].Id} Text: {questions[0].Text}" : "Keine Fragen geladen.";
        }
        #endregion

        #region Privates
        private async Task<List<IChartDataset>> GetAnswerCountByQuestion()
        {
            List<double> answerCount = new();
            List<IChartDataset> datasets = new();

            foreach(var question in questions)
            {
                int count = await _responseRepository.GetAnswerCountByQuestionIdAsync(question.Id);
                Console.WriteLine($"ID {question.Id} hat {count} Antworten");
                answerCount.Add(count);
            }

            datasets.Add(new PieChartDataset
            {
                Label = "Anzahl Antworten",
                Data = answerCount,
                BackgroundColor = ColorGenerator.CategoricalTwentyColors().ToList()
            });

            return datasets;
        }

        private List<string> LoadLabels()
        {
            return questions.Select(q => q.Text).ToList();
        }

        private async Task LoadQuestions()
        {
            questions = await _questionRepository.GetAllAsync();
        }
        #endregion
    }
}
