using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class QuestionRepository : IRepository<Question>
{
    #region Fields
    private readonly UmfrageContext _context;
    #endregion

    #region Constructors
    public QuestionRepository(UmfrageContext context)
    {
        _context = context;
    }
    #endregion

    #region Publics
    public async Task<List<Question>> GetAllAsync()
    {
        return await (_context.Questions ?? throw new InvalidOperationException()).ToListAsync(cancellationToken: CancellationToken.None);
    }

    public async Task<Question> GetByIdAsync(int id)
    {
        return await (_context.Questions ?? throw new InvalidOperationException()).FindAsync(id) ?? throw new NullReferenceException();
    }

    public async Task<List<Question>> GetAllByType(int nType)
    {
        return await (_context.Questions ?? throw new InvalidOperationException()).Where(q => q.Type == nType).ToListAsync(cancellationToken: CancellationToken.None);
    }
    #endregion
}