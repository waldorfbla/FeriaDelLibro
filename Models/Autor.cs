using System.ComponentModel.DataAnnotations;

namespace FeriaDelLibro.Models;

public class Autor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país es obligatorio")]
    [StringLength(60)]
    public string Pais { get; set; } = string.Empty;

    public List<Libro> Libros { get; set; } = new();
}