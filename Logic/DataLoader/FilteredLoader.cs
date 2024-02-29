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
        #endregion

        #region Constructors
        public FilteredLoader(IDbContextFactory<UmfrageContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        #endregion

        #region Publics
        public async Task<ChartData> LoadData(Modul? modul = null, Question? question = null)
        {
            if(modul == null && question == null) throw new ArgumentException("Modul und Frage dürfen nicht beide null sein.");

            await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
            ResponseRepository responseRepository = new(context);

            List<string> labels = new();
            List<double> answerCounts = new();

            if(question == null && modul != null) // Filter nach Modul
            {
                QuestionRepository questionRepository = new(context);
                List<Question> questions = await questionRepository.GetAllAsync();
                labels.AddRange(questions.Select(q => q.Text));

                foreach(Question q in questions)
                {
                    answerCounts.Add(await responseRepository.GetResponseCountByModuleIdAndQuestionIdAsync(modul?.Id ?? 0, q.Id));
                }
            }
            else
            {
                AnswerRepository answerRepository = new(context);
                List<Answer> answers = await answerRepository.GetByQuestionId(question.Id);
                labels.AddRange(answers.Select(a => a.Text.Split('(')[0].Trim()));

                foreach(Answer answer in answers)
                {
                    int count = modul == null
                        ? await responseRepository.GetResponseCountByAnswerIdAsync(answer.Id) // Filtern nach Frage
                        : await responseRepository.GetResponseCountByAnswerIdAndModulId(modul.Id, answer.Id); // Filtern nach Frage und Modul
                    answerCounts.Add(count);
                }
            }

            return CreateChartData(labels, answerCounts);
        }
        #endregion

        #region Privates
        private ChartData CreateChartData(List<string> labels, List<double> answerCounts)
        {
            List<IChartDataset> datasets = new()
            {
                                               new PieChartDataset
                                               {
                                                   Label = "Antwortanzahl",
                                                   Data = answerCounts,
                                                   BackgroundColor = ColorGenerator.CategoricalTwentyColors().ToList()
                                               }
                                           };

            return new ChartData
                   {
                       Labels = labels,
                       Datasets = datasets
                   };
        }
        #endregion
    }
}