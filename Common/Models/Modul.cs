using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

public class Modul
{
    #region Properties
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Sie müssen eine Modulbezeichnung zwischen 5 und 80 Zeichen angeben.")]
    [StringLength(maximumLength: 80, MinimumLength = 5)]
    public required string Name { get; set; }
    #endregion
}