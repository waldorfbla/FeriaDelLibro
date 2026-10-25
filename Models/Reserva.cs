using System.ComponentModel.DataAnnotations;

namespace FeriaDelLibro.Models;

public class Reserva
{
    public int Id { get; set; }
    public int LibroId { get; set; }
    public string TituloLibro { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe tu nombre")]
    [StringLength(60)]
    [Display(Name = "Nombre del visitante")]
    public string NombreVisitante { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Puedes reservar entre 1 y 20 ejemplares")]
    public int Cantidad { get; set; } = 1;

    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
}