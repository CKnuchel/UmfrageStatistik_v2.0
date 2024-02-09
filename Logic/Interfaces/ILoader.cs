using BlazorBootstrap;

namespace Logic.Interfaces;

public interface ILoader
{
    Task<ChartData> LoadData();
}