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

    /// <summary>
    /// Ermoeglicht das suchen nach Antworten zu einem spezifischen Semester und Jahr
    /// </summary>
    /// <param name="semester">Definieren des gewuenschten Semester 1 -> 1.Semester, 2 -> 2.Semester</param>
    /// <param name="year">Definieren des Jahres als</param>
    /// <returns>Eine Liste mit allen Responses, welceh den Filterkriterien entsprechen</returns>
    public async Task<List<Response>> GetBySemesterAndYear(int semester, int year)
    {
        if(semester < 1 || semester > 2) throw new ArgumentOutOfRangeException();
        if(year > DateTime.Now.Year) throw new ArgumentOutOfRangeException();

        if(semester == 1)
        {
            return await (_context.Responses ?? throw new InvalidOperationException()).Where(r => r.ResponseDate >= new DateTime(year, 8, 1) && r.ResponseDate <= new DateTime(year + 1, 1, 31))
                                                                                      .ToListAsync(cancellationToken: CancellationToken.None);
        }

        return await (_context.Responses ?? throw new InvalidOperationException()).Where(r => r.ResponseDate >= new DateTime(year, 2, 1) && r.ResponseDate <= new DateTime(year, 7, 31))
                                                                                  .ToListAsync(cancellationToken: CancellationToken.None);
    }
    #endregion
}