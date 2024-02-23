using BlazorBootstrap;
using Common.Models;

namespace Logic.Interfaces;

public interface IBarChartLoader
{
    // Optionale Parameter
    Task<ChartData> LoadData(Question? question = null, Modul? modul = null);
}