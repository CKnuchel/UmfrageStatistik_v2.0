using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class UmfrageContext : DbContext
{
    #region Properties
    public DbSet<Modul>? Module { get; set; }
    public DbSet<Answer>? Answers { get; set; }
    public DbSet<Question>? Questions { get; set; }
    public DbSet<Response>? Responses { get; set; }
    #endregion

    #region Constructors
    public UmfrageContext(DbContextOptions options) : base(options)
    {
    }
    #endregion
}