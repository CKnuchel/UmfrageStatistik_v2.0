using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

public class Question
{
    #region Properties
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Geben Sie einen Fragetext ein.")]
    public required string Text { get; set; }

    [Required(ErrorMessage = "Geben Sie bitte einen Fragentyp an.")]
    public int Type { get; set; }
    #endregion
}