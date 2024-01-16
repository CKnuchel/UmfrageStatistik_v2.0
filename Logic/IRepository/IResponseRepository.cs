using Common.Models;

namespace Logic.IRepository;

public interface IResponseRepository
{
    Task<List<Response>> GetAllResponseAsync();
    Task<List<Response>> GetResponseByAnswerId(int answerId);
    Task<Response> GetResponseByAnswerIdAsync(int answerId);
}