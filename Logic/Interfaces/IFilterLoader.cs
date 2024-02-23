using BlazorBootstrap;
using Common.Models;

namespace Logic.Interfaces;

public interface IFilterLoader
{
    Task<ChartData> LoadData(Modul? modul = null, Question? question = null);
}