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

    public async Task<int> GetResponseCountByQuestionIdAsync(int id)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Answer.QuestionId == id, cancellationToken: CancellationToken.None);
    }

    public async Task<int> GetResponseCountByModuleIdAndQuestionIdAsync(int modulId, int questionId)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Modul.Id == modulId && r.Answer.QuestionId == questionId, cancellationToken: CancellationToken.None);
    }

    public async Task<int> GetResponseCountByAnswerIdAsync(int id)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.AnswerId == id, CancellationToken.None);
    }

    public async Task<int> GetResponseCountByAnswerIdAndModulId(int modulId, int answerId)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.AnswerId == answerId && r.ModulId == modulId, CancellationToken.None);
    }

    public async Task<int> GetResponseCountByQuestionIdAndValue(int questionId, int nValue)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Answer.QuestionId == questionId && r.Value == nValue);
    }

    /// <summary>
    /// Ermoeglicht das suchen nach Antworten zu einem spezifischen Semester und Jahr
    /// </summary>
    /// <param name="semester">Definieren des gewuenschten Semester 1 -> 1.Semester, 2 -> 2.Semester</param>
    /// <param name="year">Definieren des Jahres als int</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>Eine Liste mit allen Responses, welche den Filterkriterien entsprechen</returns>
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