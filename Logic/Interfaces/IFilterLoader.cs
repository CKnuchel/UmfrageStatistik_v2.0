using BlazorBootstrap;
using Common.Models;

namespace Logic.Interfaces;

public interface IFilterLoader
{
    Task<ChartData> LoadData(Modul? modul);
    Task<ChartData> LoadData(Question? selectedQuestion);
}