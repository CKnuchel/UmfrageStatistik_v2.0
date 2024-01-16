using Common.Models;

namespace Logic.IRepository;

public interface IAnswerRepository
{
    Task<List<Answer>> GetAllAnswerAsync();
    Task<Answer> GetAnswerByIdAsync(int answerId);
    Task<List<Answer>> GetAnswerByQuestionIdAsync(int questionId);
}