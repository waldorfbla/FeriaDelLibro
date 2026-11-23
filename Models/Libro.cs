using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeriaDelLibro.Models;

public class Libro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un autor")]
    [Display(Name = "Autor")]
    public int AutorId { get; set; }          // clave foránea
    public Autor? Autor { get; set; }         // propiedad de navegación

    [Required(ErrorMessage = "Selecciona un género")]
    public string Genero { get; set; } = string.Empty;

    [Range(1000, 1000000, ErrorMessage = "El precio debe estar entre $1.000 y $1.000.000")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [Range(0, 500, ErrorMessage = "El stock debe estar entre 0 y 500")]
    public int Stock { get; set; }

    [Display(Name = "Fecha de publicación")]
    [DataType(DataType.Date)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Today;

    public List<Reserva> Reservas { get; set; } = new();
}