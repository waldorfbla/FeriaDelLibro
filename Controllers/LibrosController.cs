using FeriaDelLibro.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeriaDelLibro.Controllers;

public class LibrosController : Controller
{
    // "Base de datos" en memoria
    private static readonly List<Libro> _libros = new()
    {
        new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Genero = "Novela", Precio = 45000, Stock = 10, FechaPublicacion = new DateTime(1967, 5, 30) },
        new Libro { Id = 2, Titulo = "El amor en los tiempos del cólera", Autor = "Gabriel García Márquez", Genero = "Novela", Precio = 42000, Stock = 8, FechaPublicacion = new DateTime(1985, 12, 5) },
        new Libro { Id = 3, Titulo = "La vorágine", Autor = "José Eustasio Rivera", Genero = "Clásico", Precio = 30000, Stock = 5, FechaPublicacion = new DateTime(1924, 1, 1) }
    };
    private static readonly List<Reserva> _reservas = new();
    private static int _siguienteLibroId = 4;
    private static int _siguienteReservaId = 1;

    // GET: /Libros
    public IActionResult Index()
    {
        return View(_libros);
    }

    // GET: /Libros/Crear
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Libro());
    }

    // POST: /Libros/Crear
    [HttpPost]
    public IActionResult Crear(Libro libro)
    {
        if (!ModelState.IsValid)
            return View(libro);

        libro.Id = _siguienteLibroId++;
        _libros.Add(libro);

        TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" fue registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Libros/Reservar/1
    [HttpGet]
    public IActionResult Reservar(int id)
    {
        var libro = _libros.FirstOrDefault(l => l.Id == id);
        if (libro == null) return NotFound();

        ViewBag.Libro = libro;
        return View(new Reserva { LibroId = libro.Id, TituloLibro = libro.Titulo });
    }

    // POST: /Libros/Reservar
    [HttpPost]
    public IActionResult Reservar(Reserva reserva)
    {
        var libro = _libros.FirstOrDefault(l => l.Id == reserva.LibroId);
        if (libro == null) return NotFound();

        if (reserva.Cantidad > libro.Stock)
            ModelState.AddModelError(nameof(reserva.Cantidad), $"Solo quedan {libro.Stock} ejemplares disponibles.");

        if (!ModelState.IsValid)
        {
            ViewBag.Libro = libro;
            return View(reserva);
        }

        // Procesamiento en C#: descuento del 10% desde 3 ejemplares
        decimal subtotal = libro.Precio * reserva.Cantidad;
        reserva.Descuento = reserva.Cantidad >= 3 ? subtotal * 0.10m : 0;
        reserva.Total = subtotal - reserva.Descuento;
        reserva.TituloLibro = libro.Titulo;
        reserva.Id = _siguienteReservaId++;
        reserva.Fecha = DateTime.Now;

        libro.Stock -= reserva.Cantidad;
        _reservas.Add(reserva);

        return RedirectToAction(nameof(Confirmacion), new { id = reserva.Id });
    }

    // GET: /Libros/Confirmacion/1
    public IActionResult Confirmacion(int id)
    {
        var reserva = _reservas.FirstOrDefault(r => r.Id == id);
        if (reserva == null) return NotFound();

        return View(reserva);
    }
}