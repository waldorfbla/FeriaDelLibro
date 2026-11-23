using FeriaDelLibro.Data;
using FeriaDelLibro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FeriaDelLibro.Controllers;

public class LibrosController : Controller
{
    private readonly FeriaContext _context;

    public LibrosController(FeriaContext context)
    {
        _context = context;
    }

    // Llena la lista desplegable de autores
    private async Task CargarAutores(int? seleccionado = null)
    {
        var autores = await _context.Autores.OrderBy(a => a.Nombre).ToListAsync();
        ViewBag.Autores = new SelectList(autores, "Id", "Nombre", seleccionado);
    }

    // READ: catálogo con el autor de cada libro (consulta con Include)
    public async Task<IActionResult> Index()
    {
        var libros = await _context.Libros
            .Include(l => l.Autor)
            .OrderBy(l => l.Titulo)
            .ToListAsync();
        return View(libros);
    }

    // CREATE
    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await CargarAutores();
        return View(new Libro());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutores(libro.AutorId);
            return View(libro);
        }

        libro.Id = 0; // CAMBIO: la base de datos genera el Id
        _context.Libros.Add(libro);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" fue registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // UPDATE
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var libro = await _context.Libros.FindAsync(id);
        if (libro == null) return NotFound();

        await CargarAutores(libro.AutorId);
        return View(libro);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutores(libro.AutorId);
            return View(libro);
        }

        _context.Libros.Update(libro);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" fue actualizado.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var libro = await _context.Libros
            .Include(l => l.Autor)
            .Include(l => l.Reservas)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (libro == null) return NotFound();

        return View(libro);
    }

    [HttpPost, ActionName("Eliminar")]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var libro = await _context.Libros.FindAsync(id);
        if (libro != null)
        {
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" fue eliminado.";
        }
        return RedirectToAction(nameof(Index));
    }

    // RESERVAR (GET)
    [HttpGet]
    public async Task<IActionResult> Reservar(int id)
    {
        var libro = await _context.Libros.Include(l => l.Autor).FirstOrDefaultAsync(l => l.Id == id);
        if (libro == null) return NotFound();

        ViewBag.Libro = libro;
        return View(new Reserva { LibroId = libro.Id });
    }

    // RESERVAR (POST): procesa en C# y guarda en la base de datos
    [HttpPost]
    public async Task<IActionResult> Reservar(Reserva reserva)
    {
        reserva.Id = 0; // CAMBIO: evita que el id del libro en la URL se use como Id de la reserva

        var libro = await _context.Libros.Include(l => l.Autor).FirstOrDefaultAsync(l => l.Id == reserva.LibroId);
        if (libro == null) return NotFound();

        if (reserva.Cantidad > libro.Stock)
            ModelState.AddModelError(nameof(reserva.Cantidad), $"Solo quedan {libro.Stock} ejemplares disponibles.");

        if (!ModelState.IsValid)
        {
            ViewBag.Libro = libro;
            return View(reserva);
        }

        // Descuento del 10% desde 3 ejemplares
        decimal subtotal = libro.Precio * reserva.Cantidad;
        reserva.Descuento = reserva.Cantidad >= 3 ? subtotal * 0.10m : 0;
        reserva.Total = subtotal - reserva.Descuento;
        reserva.Fecha = DateTime.Now;

        libro.Stock -= reserva.Cantidad;
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Confirmacion), new { id = reserva.Id });
    }

    // CONFIRMACIÓN (consulta con datos del libro relacionado)
    public async Task<IActionResult> Confirmacion(int id)
    {
        var reserva = await _context.Reservas.Include(r => r.Libro).FirstOrDefaultAsync(r => r.Id == id);
        if (reserva == null) return NotFound();

        return View(reserva);
    }
}