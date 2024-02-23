using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class AnswerRepository : IRepository<Answer>
{
    #region Fields
    private readonly UmfrageContext _context;
    #endregion

    #region Constructors
    public AnswerRepository(UmfrageContext context)
    {
        _context = context;
    }
    #endregion

    #region Publics
    public async Task<List<Answer>> GetAllAsync()
    {
        return await (_context.Answers ?? throw new InvalidOperationException()).ToListAsync(cancellationToken: CancellationToken.None);
    }

    public async Task<List<Answer>> GetByQuestionId(int nId)
    {
        return await (_context.Answers ?? throw new InvalidOperationException()).Where(a => a.QuestionId == nId).ToListAsync() ?? throw new NullReferenceException();
    }
    #endregion
}