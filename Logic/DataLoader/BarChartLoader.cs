using BlazorBootstrap;
using Data.Context;
using Logic.Repository;

namespace Logic.DataLoader;

public class BarChartLoader
{
    private ResponseRepository _responseRepository;


    public BarChartLoader(UmfrageContext context)
    {
        _responseRepository = new ResponseRepository(context);
    }

    public async Task<ChartData> LoadData()
    {
        throw new NotImplementedException();
    }
}