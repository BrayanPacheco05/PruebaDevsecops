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
        var contactos = await _context.Contacto
            .Where(c => c.Activo)
            .Include(c => c.ContactosTelefonos)
                .ThenInclude(ct => ct.Telefono)
            .ToListAsync();

        return Ok(contactos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var contacto = await _context.Contacto
            .Where(c => c.Id == id && c.Activo)
            .Include(c => c.ContactosTelefonos)
                .ThenInclude(ct => ct.Telefono)
            .FirstOrDefaultAsync();

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
            .Where(c => c.Activo && c.Nombre.Contains(nombre))
            .Include(c => c.ContactosTelefonos)
                .ThenInclude(ct => ct.Telefono)
            .ToListAsync();

        return Ok(contactos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(
        int id,
        [FromBody] ContactoModel datos)
    {
        var contacto = await _context.Contacto
            .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

        if (contacto == null)
            return NotFound();

        var emailExiste = await _context.Contacto
            .AnyAsync(c => c.Id != id && c.Email == datos.Email);

        if (emailExiste)
            return Conflict("El email ya está registrado.");

        contacto.Nombre = datos.Nombre;
        contacto.Email = datos.Email;

        await _context.SaveChangesAsync();

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
        var emailExiste = await _context.Contacto
            .AnyAsync(c => c.Email == contacto.Email);

        if (emailExiste)
            return Conflict("El email ya está registrado.");

        contacto.Id = 0;
        contacto.FechaCreacion = DateTime.UtcNow;
        contacto.Activo = true;

        _context.Contacto.Add(contacto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = contacto.Id },
            contacto);
    }
}