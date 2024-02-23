using BlazorBootstrap;

namespace Logic.Interfaces;

public interface ISemesterLoader
{
    Task<ChartData> LoadData();
    Task<ChartData> LoadDataByModul(int nModulId);
}