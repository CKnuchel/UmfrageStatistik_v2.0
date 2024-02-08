using BlazorBootstrap;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Logic.DataLoader;

public interface ILoader
{
    Task<ChartData> LoadData();
}