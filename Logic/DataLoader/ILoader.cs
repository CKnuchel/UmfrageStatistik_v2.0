using BlazorBootstrap;

namespace Logic.DataLoader;

public interface ILoader
{
    Task<ChartData> LoadData();
}