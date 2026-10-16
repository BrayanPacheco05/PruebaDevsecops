using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSharpConctactApi.Data;
using CSharpConctactApi.Models;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{

    private readonly AppDbContext _context;

    public ContactController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contactos = await _context.Contacto.ToListAsync();

        
        return Ok(contactos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var contacto = await _context.Contacto.FindAsync(id);

        if (contacto == null ) {
            return NotFound();
        }

        return Ok(contacto);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> GetByNombre([FromQuery] string nombre)
    {

        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest("nombre invalido");

        var contactos = await _context.Contacto
        .Where(c => EF.Functions.ILike(c.Nombre, $"%{nombre}%"))
        .ToListAsync();

        return Ok(contactos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromQuery] ContactoModel contacto)
    {

        if (id != contacto.Id)
            return BadRequest("El id no coincide ");

        var busqueda = await _context.Contacto.AnyAsync(c => c.Id == id);

        if (!busqueda)
            return NotFound($"no existe un contacto con ese id {id}");

        _context.Entry(contacto).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            return StatusCode(500, "error interno");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {

        var contacto = await _context.Contacto.FindAsync(id);

        if (contacto == null || !contacto.Activo) {
            return NotFound($"El contacto no fue encontrado");
        }

        contacto.Activo = false;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddContact([FromBody] ContactoModel contacto)
    {
        if (contacto == null ){
            return BadRequest("contacto invalido");
        }

        _context.Contacto.Add(contacto);
        await _context.SaveChangesAsync();

        return Created("", contacto);
    }
}