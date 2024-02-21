using BlazorBootstrap;

namespace Logic.Interfaces;

public interface ISemesterLoader
{
    Task<ChartData> LoadData();
    Task<List<int>> GetAvailableYears();
}