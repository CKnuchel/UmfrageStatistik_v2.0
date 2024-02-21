namespace Logic.Interfaces;

public interface ISemesterLoader
{
    Task<List<int>> GetAvailableYears();
}