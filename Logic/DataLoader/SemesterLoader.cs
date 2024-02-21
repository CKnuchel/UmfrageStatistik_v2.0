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
    private IList<int> years = new List<int>();
    private readonly List<string> labels = new List<string>();
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

        //Jahre auslesen
        years = await responseRepository.GetAvailableYearsFromResponses();

        throw new NotImplementedException();
    }

    public async Task<List<int>> GetAvailableYears()
    {
        await using UmfrageContext context = await _contextFactory.CreateDbContextAsync();
        ResponseRepository responseRepository = new(context);

        return await responseRepository.GetAvailableYearsFromResponses();
    }
    #endregion

    #region Privates
    private void CreateLabels()
    {
        if(years.Count ! > 0) throw new ArgumentException(nameof(years));

        foreach(int year in years)
        {
            // TODO Hinzufügen der Labels S1Y16, S2Y16, ...
        }
    }
    #endregion
}