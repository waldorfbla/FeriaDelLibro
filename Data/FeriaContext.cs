using FeriaDelLibro.Models;
using Microsoft.EntityFrameworkCore;

namespace FeriaDelLibro.Data;

public class FeriaContext : DbContext
{
    public FeriaContext(DbContextOptions<FeriaContext> options) : base(options) { }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Datos iniciales
        modelBuilder.Entity<Autor>().HasData(
            new Autor { Id = 1, Nombre = "Gabriel García Márquez", Pais = "Colombia" },
            new Autor { Id = 2, Nombre = "José Eustasio Rivera", Pais = "Colombia" }
        );

        modelBuilder.Entity<Libro>().HasData(
            new Libro { Id = 1, Titulo = "Cien años de soledad", AutorId = 1, Genero = "Novela", Precio = 45000, Stock = 10, FechaPublicacion = new DateTime(1967, 5, 30) },
            new Libro { Id = 2, Titulo = "El amor en los tiempos del cólera", AutorId = 1, Genero = "Novela", Precio = 42000, Stock = 8, FechaPublicacion = new DateTime(1985, 12, 5) },
            new Libro { Id = 3, Titulo = "La vorágine", AutorId = 2, Genero = "Clásico", Precio = 30000, Stock = 5, FechaPublicacion = new DateTime(1924, 1, 1) }
        );
    }
}