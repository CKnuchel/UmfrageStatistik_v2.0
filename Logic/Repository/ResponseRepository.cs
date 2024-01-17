using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class ResponseRepository : IRepository<Response>
{
    #region Fields
    private readonly UmfrageContext _context;
    #endregion

    #region Constructors
    public ResponseRepository(UmfrageContext context)
    {
        _context = context;
    }
    #endregion

    #region Publics
    public async Task<List<Response>> GetAllAsync()
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).ToListAsync(cancellationToken: CancellationToken.None);
    }

    public async Task<Response> GetByIdAsync(int id)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).FindAsync(id) ?? throw new NullReferenceException();
    }

    public async Task<List<Response>> GetByAnswerIdAsync(int id)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).Where(r => r.AnswerId == id).ToListAsync(cancellationToken: CancellationToken.None);
    }

    public async Task<List<Response>> GetByModuleIdAsync(int id)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).Where(r => r.ModulId == id).ToListAsync(cancellationToken: CancellationToken.None);
    }

    public async Task<List<Response>> GetByQuestionIdAsync(int id)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).Where(r => r.Answer.QuestionId == id).ToListAsync(cancellationToken: CancellationToken.None);
    }
    #endregion
}