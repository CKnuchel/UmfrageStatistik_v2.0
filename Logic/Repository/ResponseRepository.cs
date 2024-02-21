using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class ResponseRepository : IRepository<Response>
{
    #region Constants
    private static readonly Dictionary<int, (int startMonth, int startDay, int endMonth, int endDay)> SemesterDates = new()
                                                                                                                      {
                                                                                                                          { 1, (8, 1, 1, 31) }, // 1. Semester von 1. August bis 31. Januar
                                                                                                                          { 2, (2, 1, 7, 31) } // 2. Semester von 1. Februar bis 31. Juli
                                                                                                                      };
    #endregion

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

    public async Task<int> GetResponseCountByQuestionTypeAndValue(int nValue, int nType)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Answer.Question.Type == nType && r.Value == nValue, CancellationToken.None);
    }

    public async Task<int> GetResponseCountByQuesionIdAndModulIdAndValue(int modulId, int questionId, int nValue)
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).CountAsync(r => r.Answer.QuestionId == questionId && r.ModulId == modulId && r.Value == nValue, CancellationToken.None);
    }

    /// <summary>
    /// Ermittelt die Anzahl Antworten für eine Frage, bezogen auf das Semester und Jahr
    /// </summary>
    /// <param name="semester"> 1 = erstes Semester, 2 = zweites Semester</param>
    /// <param name="year">Das Jahr, welches für die Abfrage relevant ist</param>
    /// <param name="questionId">Zu welcher Frage die Daten geladen werden sollen</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Bei Fehlerhaften Angaben (ausserhalb des erlaubten Bereich)</exception>
    /// <exception cref="InvalidOperationException">Kein Kontext oder Tabelle gefunden</exception>
    public async Task<int> GetResponseCountByQuestionIdSemesterAndYear(int semester, int year, int questionId)
    {
        if(semester is < 1 or > 2) throw new ArgumentOutOfRangeException(nameof(semester), "Semester muss 1 oder 2 sein.");
        if(year < 0 || year > DateTime.Now.Year) throw new ArgumentOutOfRangeException(nameof(year), "Jahr muss positiv sein und darf das aktuelle Jahr nicht überschreiten.");

        (int startMonth, int startDay, int endMonth, int endDay) = SemesterDates[semester];
        DateTime startDate = new(year, startMonth, startDay);
        DateTime endDate = semester == 1 ? new DateTime(year + 1, endMonth, endDay) : new DateTime(year, endMonth, endDay);

        return await (_context.Responses ?? throw new InvalidOperationException())
                     .Where(r => r.ResponseDate >= startDate && r.ResponseDate <= endDate && r.Answer.QuestionId == questionId)
                     .CountAsync();
    }

    /// <summary>
    /// Ermittelt die Anzahl Antworten für eine Frage, bezogen auf das Semester, Jahr und Modul
    /// </summary>
    /// <param name="semester"> 1 = erstes Semester, 2 = zweites Semester</param>
    /// <param name="year">Das Jahr, welches für die Abfrage relevant ist</param>
    /// <param name="questionId">Zu welcher Frage die Daten geladen werden sollen</param>
    /// <param name="modulId">Die Modul Id zum Filtern der Antworten</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Bei Fehlerhaften Angaben (ausserhalb des erlaubten Bereich)</exception>
    /// <exception cref="InvalidOperationException">Kein Kontext oder Tabelle gefunden</exception>
    public async Task<int> GetResponseCountByQuestionIdAndModulIdSemesterAndYear(int semester, int year, int questionId, int modulId)
    {
        if(semester is < 1 or > 2) throw new ArgumentOutOfRangeException(nameof(semester), "Semester muss 1 oder 2 sein.");
        if(year < 0 || year > DateTime.Now.Year) throw new ArgumentOutOfRangeException(nameof(year), "Jahr muss positiv sein und darf das aktuelle Jahr nicht überschreiten.");

        (int startMonth, int startDay, int endMonth, int endDay) = SemesterDates[semester];
        DateTime startDate = new(year, startMonth, startDay);
        DateTime endDate = semester == 1 ? new DateTime(year + 1, endMonth, endDay) : new DateTime(year, endMonth, endDay);

        return await (_context.Responses ?? throw new InvalidOperationException())
                     .Where(r => r.ResponseDate >= startDate && r.ResponseDate <= endDate && r.Answer.QuestionId == questionId && r.ModulId == modulId)
                     .CountAsync();
    }

    public async Task<List<int>> GetAvailableYearsFromResponses()
    {
        return await (_context.Responses ?? throw new InvalidOperationException()).Select(r => r.ResponseDate.Year).Distinct().ToListAsync(cancellationToken: CancellationToken.None);
    }
    #endregion
}