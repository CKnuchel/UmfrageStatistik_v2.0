using System.Reflection.Metadata.Ecma335;
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

    public async Task<Answer> GetByIdAsync(int id)
    {
        return await (_context.Answers ?? throw new InvalidOperationException()).FindAsync(id) ?? throw new NullReferenceException();
    }

    public async Task<List<Answer>> GetByQuestionId(int id)
    {
        return await (_context.Answers ?? throw new InvalidOperationException()).Where(a => a.QuestionId == id).ToListAsync() ?? throw new NullReferenceException();
    }

    #endregion
}