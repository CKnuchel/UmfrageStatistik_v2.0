using BlazorBootstrap;
using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;

namespace Logic.DataLoader
{
    public class FilteredLoader : ILoader
    {

        private IList<Question> questions = new List<Question>();
        private readonly QuestionRepository _questionRepository;
        private readonly ResponseRepository _responseRepository;

        public FilteredLoader(UmfrageContext context)
        {
            _questionRepository = new QuestionRepository(context);
            _responseRepository = new ResponseRepository(context);
        }

        public async Task<ChartData> LoadData()
        {
            throw new NotImplementedException();
        }

    }
}
