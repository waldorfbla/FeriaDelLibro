using FeriaDelLibro.Data;
using FeriaDelLibro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeriaDelLibro.Controllers;

public class AutoresController : Controller
{
    private readonly FeriaContext _context;

    public AutoresController(FeriaContext context)
    {
        _context = context;
    }

    // READ: lista de autores con sus libros
    public async Task<IActionResult> Index()
    {
        var autores = await _context.Autores.Include(a => a.Libros).ToListAsync();
        return View(autores);
    }

    // CREATE
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Autor());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Autor autor)
    {
        if (!ModelState.IsValid) return View(autor);

        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Autor \"{autor.Nombre}\" creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // UPDATE
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null) return NotFound();
        return View(autor);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Autor autor)
    {
        if (!ModelState.IsValid) return View(autor);

        _context.Autores.Update(autor);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Autor \"{autor.Nombre}\" actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var autor = await _context.Autores.Include(a => a.Libros).FirstOrDefaultAsync(a => a.Id == id);
        if (autor == null) return NotFound();
        return View(autor);
    }

    [HttpPost, ActionName("Eliminar")]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor != null)
        {
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"Autor \"{autor.Nombre}\" eliminado.";
        }
        return RedirectToAction(nameof(Index));
    }
}