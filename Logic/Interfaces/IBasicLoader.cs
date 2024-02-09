using BlazorBootstrap;

namespace Logic.Interfaces;

public interface IBasicLoader
{
    Task<ChartData> LoadData();
}