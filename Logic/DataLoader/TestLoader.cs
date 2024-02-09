using BlazorBootstrap;
using Common;
using Common.Models;
using Logic.Interfaces;

namespace Logic.DataLoader
{
    public class TestLoader : ILoader
    {
        #region Fields
        private readonly IList<Question> questions = new List<Question>();
        #endregion

        #region Publics
        public Task<ChartData> LoadData()
        {
            LoadQuestions();

            return Task.FromResult(new ChartData
                                   {
                                       Labels = LoadLabels(),
                                       Datasets = GetAnswerCountByQuestion().Result
                                   });
        }
        #endregion

        #region Privates
        private Task<List<IChartDataset>> GetAnswerCountByQuestion()
        {
            List<double> answerCount = new();
            List<IChartDataset> dataset = new();

            answerCount.Add(500);
            answerCount.Add(600);
            answerCount.Add(732);
            answerCount.Add(253);
            answerCount.Add(459);

            dataset.Add(new PieChartDataset
                        {
                            Label = "Anzahl Antworten",
                            Data = answerCount,
                            BackgroundColor = ColorGenerator.CategoricalTwentyColors().ToList()
                        });

            return Task.FromResult(dataset);
        }

        private List<string> LoadLabels()
        {
            return questions.Select(q => q.Text).ToList();
        }

        private void LoadQuestions()
        {
            Question q1 = new() { Id = 1, Text = "Test 1", Type = 1 };
            Question q2 = new() { Id = 2, Text = "Test 2", Type = 1 };
            Question q3 = new() { Id = 3, Text = "Test 3", Type = 1 };
            Question q4 = new() { Id = 4, Text = "Test 4", Type = 1 };
            Question q5 = new() { Id = 5, Text = "Test 5", Type = 1 };

            questions.Add(q1);
            questions.Add(q2);
            questions.Add(q3);
            questions.Add(q4);
            questions.Add(q5);
        }
        #endregion
    }
}