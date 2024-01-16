using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class UmfrageContext : DbContext
{
    #region Properties
    // Einbinden der Models in den Kontext
    public DbSet<Modul>? Module { get; set; }
    public DbSet<Answer>? Answers { get; set; }
    public DbSet<Question>? Questions { get; set; }
    public DbSet<Response>? Responses { get; set; }
    #endregion

    #region Constructors
    public UmfrageContext(DbContextOptions<UmfrageContext> options) : base(options)
    {
    }
    #endregion
}