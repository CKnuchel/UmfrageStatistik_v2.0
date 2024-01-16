using Common.Models;

namespace Logic.IRepository;

public interface IQuestionRepository
{
    Task<List<Question>> GetAllQuestionAsync();
    Task<Question> GetQuestionByIdAsync(int questionId);
    Task<List<Question>> GetQuestionByTypeAsync(int type);
}