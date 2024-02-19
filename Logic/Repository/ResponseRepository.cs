using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class ResponseRepository : IRepository<Response>
{
    #region Fields
    // Definiere Konstanten für die Semesterdaten
    private static readonly Dictionary<int, (int startMonth, int startDay, int endMonth, int endDay)> SemesterDates = new Dictionary<int, (int, int, int, int)>
                                                                                                                      {
                                                                                                                          { 1, (8, 1, 1, 31) }, // 1. Semester von 1. August bis 31. Januar
                                                                                                                          { 2, (2, 1, 7, 31) } // 2. Semester von 1. Februar bis 31. Juli
                                                                                                                      };

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

    public async Task<int> GetResponseCountByQuestionTypeAndValue(int nValue, int nType)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Answer.Question.Type == nType && r.Value == nValue, CancellationToken.None);
    }

    public async Task<int> GetResponseCountByQuesionIdAndModulIdAndValue(int modulId, int questionId, int nValue)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Answer.QuestionId == questionId && r.ModulId == modulId && r.Value == nValue, CancellationToken.None);
    }

    /// <summary>
    /// Ermöglicht das Suchen nach Antworten zu einem spezifischen Semester und Jahr
    /// </summary>
    /// <param name="semester">Definieren des gewünschten Semesters 1 -> 1.Semester, 2 -> 2.Semester</param>
    /// <param name="year">Definieren des Jahres als int</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>Eine Liste mit allen Responses, welche den Filterkriterien entsprechen</returns>
    public async Task<List<Response>> GetBySemesterAndYear(int semester, int year)
    {
        if(semester < 1 || semester > 2) throw new ArgumentOutOfRangeException(nameof(semester), "Semester muss 1 oder 2 sein.");
        if(year < 0 || year > DateTime.Now.Year) throw new ArgumentOutOfRangeException(nameof(year), "Jahr muss positiv sein und darf das aktuelle Jahr nicht überschreiten.");

        (int startMonth, int startDay, int endMonth, int endDay) = SemesterDates[semester];
        DateTime startDate = new DateTime(year, startMonth, startDay);
        DateTime endDate = semester == 1 ? new DateTime(year + 1, endMonth, endDay) : new DateTime(year, endMonth, endDay);

        return await (_context.Responses ?? throw new InvalidOperationException())
                     .Where(r => r.ResponseDate >= startDate && r.ResponseDate <= endDate)
                     .ToListAsync(cancellationToken: CancellationToken.None);
    }
    #endregion
}