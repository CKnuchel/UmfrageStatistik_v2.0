using BlazorBootstrap;
using Data.Context;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Logic.DataLoader;

public class SemesterLoader : ISemesterLoader
{
    #region Fields
    private readonly IDbContextFactory<UmfrageContext> _contextFactory;
    #endregion

    #region Constructors
    public SemesterLoader(IDbContextFactory<UmfrageContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    #endregion

    #region Publics
    public async Task<ChartData> LoadData()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        throw new NotImplementedException();
    }

    public async Task<List<int>> GetAvailableYears()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return await responseRepository.GetAvailableYearsFromResponses();
    }
    #endregion
}