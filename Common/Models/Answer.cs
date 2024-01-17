using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

public class Answer
{
    #region Properties
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Bitte geben Sie die Fragen an, auf welche sich die Antwort bezieht.")]
    public required int QuestionId { get; set; }

    [Required]
    public required string Text { get; set; }

    // Referenzierungen für das "Binding" der Relationen
    public Question Question { get; set; } = null!;
    #endregion
}