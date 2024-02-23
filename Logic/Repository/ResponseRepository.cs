using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class ResponseRepository : IRepository<Response>
{
    #region Constants
    private static readonly Dictionary<int, (int nStartMonth, int nStartDay, int nEndMonth, int nEndDay)> SemesterDates = new()
                                                                                                                          {
                                                                                                                              { 1, (2, 1, 7, 31) }, // Semester von 1. Februar bis 31. Juli
                                                                                                                              { 2, (8, 1, 1, 31) } // Semester von 1. August bis 31. Januar
                                                                                                                          };

    private const string DbContextErrorMessage = "Der Datenkontext darf nicht null sein.";
    #endregion

    #region Fields
    private readonly UmfrageContext _context;
    #endregion

    #region Constructors
    public ResponseRepository(UmfrageContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context), DbContextErrorMessage);
    }
    #endregion

    #region Publics
    public async Task<List<Response>> GetAllAsync()
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).ToListAsync();
    }

    public async Task<int> GetResponseCountByQuestionIdAsync(int nQuestionId)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.Answer.QuestionId == nQuestionId);
    }

    public async Task<int> GetResponseCountByModuleIdAndQuestionIdAsync(int nModuleId, int nQuestionId)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.Modul.Id == nModuleId && r.Answer.QuestionId == nQuestionId);
    }

    public async Task<int> GetResponseCountByAnswerIdAsync(int nAnswerId)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.AnswerId == nAnswerId);
    }

    public async Task<int> GetResponseCountByAnswerIdAndModulId(int nModuleId, int nAnswerId)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.AnswerId == nAnswerId && r.ModulId == nModuleId);
    }

    public async Task<int> GetResponseCountByQuestionIdAndValue(int nQuestionId, int nValue)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.Answer.QuestionId == nQuestionId && r.Value == nValue);
    }

    public async Task<int> GetResponseCountByQuesionIdAndModulIdAndValue(int nModulId, int nQuestionId, int nValue)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.Answer.QuestionId == nQuestionId && r.ModulId == nModulId && r.Value == nValue);
    }

    public async Task<int> GetResponseCountByQuestionTypeAndValue(int nValue, int nType)
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).CountAsync(r => r.Answer.Question.Type == nType && r.Value == nValue);
    }

    public async Task<int> GetResponseCountByQuestionIdAndSemesterAndYear(int nSemester, int nYear, int nQuestionId)
    {
        IQueryable<Response> query = GetFilteredResponsesByDateAndOptionallyModule(nSemester, nYear);
        return await query.CountAsync(r => r.Answer.QuestionId == nQuestionId);
    }

    public async Task<int> GetResponseCountByQuestionIdAndModulIdSemesterAndYear(int nSemester, int nYear, int nQuestionId, int nModuleId)
    {
        IQueryable<Response> query = GetFilteredResponsesByDateAndOptionallyModule(nSemester, nYear, nModuleId);
        return await query.CountAsync(r => r.Answer.QuestionId == nQuestionId);
    }

    public async Task<bool> IsSemesterDataAvailable(int nYear, int nSemester)
    {
        IQueryable<Response> query = GetFilteredResponsesByDateAndOptionallyModule(nSemester, nYear);
        return await query.AnyAsync();
    }

    public async Task<bool> IsSemesterDataAvailableByModuleId(int nYear, int nSemester, int nModuleId)
    {
        IQueryable<Response> query = GetFilteredResponsesByDateAndOptionallyModule(nSemester, nYear, nModuleId);
        return await query.AnyAsync();
    }

    public async Task<List<int>> GetAvailableYearsFromResponses()
    {
        return await (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).Select(r => r.ResponseDate.Year).Distinct().ToListAsync();
    }
    #endregion

    #region Privates
    private IQueryable<Response> GetFilteredResponsesByDateAndOptionallyModule(int nSemester, int nYear, int? nModuleId = null)
    {
        (DateTime startDate, DateTime endDate) = GetSemesterDateRange(nSemester, nYear);
        IQueryable<Response> query = (_context.Responses ?? throw new InvalidOperationException(DbContextErrorMessage)).Where(r => r.ResponseDate >= startDate && r.ResponseDate <= endDate);

        if(nModuleId.HasValue)
        {
            query = query.Where(r => r.ModulId == nModuleId.Value);
        }

        return query;
    }

    private (DateTime dtStartDate, DateTime dtEndDate) GetSemesterDateRange(int nSemester, int nYear)
    {
        (int nStartMonth, int nStartDay, int nEndMonth, int nEndDay) = SemesterDates[nSemester];
        DateTime dtStartDate = new(nYear, nStartMonth, nStartDay);
        DateTime dtEndDate = nSemester == 2 ? new DateTime(nYear + 1, nEndMonth, nEndDay) : new DateTime(nYear, nEndMonth, nEndDay);

        return (dtStartDate, dtEndDate);
    }
    #endregion
}