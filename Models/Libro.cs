using System.ComponentModel.DataAnnotations;

namespace FeriaDelLibro.Models;

public class Libro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El autor es obligatorio")]
    [StringLength(80)]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona un género")]
    public string Genero { get; set; } = string.Empty;

    [Range(1000, 1000000, ErrorMessage = "El precio debe estar entre $1.000 y $1.000.000")]
    public decimal Precio { get; set; }

    [Range(0, 500, ErrorMessage = "El stock debe estar entre 0 y 500")]
    public int Stock { get; set; }

    [Display(Name = "Fecha de publicación")]
    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Today;
}
