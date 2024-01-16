using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

public class Response
{
    #region Properties
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Bitte wählen Sie eine Antwort aus.")]
    public int AnswerId { get; set; }

    [Required(ErrorMessage = "Wählen Sie ein Modul aus.")]
    public int ModulId { get; set; }

    [Required(ErrorMessage = "Bitte geben Sie den aktuellen Timestamp an.")]
    [DataType(DataType.DateTime)]
    public DateTime ResponseDate { get; set; }

    [Required(ErrorMessage = "Bitte geben Sie den Antwort Wert an.")]
    [Range(0, 10)]
    public int Value { get; set; }

    // Referenzierungen für das "Binding" der Relationen
    public Answer Answer { get; set; } = null!;
    public Modul Modul { get; set; } = null!;
    #endregion
}