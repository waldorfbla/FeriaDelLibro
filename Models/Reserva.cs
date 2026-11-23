using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeriaDelLibro.Models;

public class Reserva
{
    public int Id { get; set; }

    public int LibroId { get; set; }          // clave foránea
    public Libro? Libro { get; set; }         // propiedad de navegación

    [Required(ErrorMessage = "Escribe tu nombre")]
    [StringLength(60)]
    [Display(Name = "Nombre del visitante")]
    public string NombreVisitante { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Puedes reservar entre 1 y 20 ejemplares")]
    public int Cantidad { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Descuento { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;
}