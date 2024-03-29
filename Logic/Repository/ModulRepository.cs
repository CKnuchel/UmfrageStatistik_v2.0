﻿using Common.Models;
using Data.Context;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logic.Repository;

public class ModulRepository : IRepository<Modul>
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
    public async Task<List<Modul>> GetAllAsync()
    {
        return await (_context.Module ?? throw new InvalidOperationException()).ToListAsync(cancellationToken: CancellationToken.None);
    }
    #endregion
}