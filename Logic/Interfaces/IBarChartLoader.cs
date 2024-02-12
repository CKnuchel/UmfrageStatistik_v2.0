using BlazorBootstrap;
using Common.Models;

namespace Logic.Interfaces;

public interface IBarChartLoader
{
    public Task<ChartData> LoadData(Question question);
}