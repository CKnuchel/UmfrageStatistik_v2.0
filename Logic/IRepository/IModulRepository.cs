using Common.Models;

namespace Logic.IRepository;

public interface IModulRepository
{
    Task<List<Modul>> GetAllModuleAsync();
    Task<Modul> GetModuleByIdAsync(int modulId);
}