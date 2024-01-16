using Common.Models;
using Data.Context;
using Logic.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class ModulRepository : IModulRepository
{
    #region Fields
    private readonly UmfrageContext _context;
    #endregion

    #region Constructors
    public ModulRepository(UmfrageContext context)
    {
        _context = context;
    }
    #endregion

    #region Publics
    public async Task<List<Modul>> GetAllModuleAsync()
    {
        return await _context.Module.ToListAsync() ?? throw new NullReferenceException("Es existieren keine Einträge");
    }

    public async Task<Modul> GetModuleByIdAsync(int modulId)
    {
        return await _context.Module.FindAsync(modulId) ?? throw new NullReferenceException("Diese Id existiert nicht.");
    }
    #endregion
}