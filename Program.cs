using Microsoft.EntityFrameworkCore;
using CSharpConctactApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION")
        ?? throw new InvalidOperationException(
            "Falta la variable de entorno DB_CONNECTION"
        )
    ));
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider
		.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();


var contactos = new List<Contacto>
{
    new(1, "bob", "bo@gs.com", "443447231")
};

app.MapGet("/api/contactos", () =>
{
    
    
    return contactos;
});

app.MapPost("/api/contactos", (Contacto contacto) => {

    contactos.Add(contacto);

    return Results.Created($"", contacto);

});

app.MapDelete("api/contactos/{id:int}", (int id) => 
{
    var contacto = contactos.FirstOrDefault(c => c.Id == id);

    if (contacto == null ) {
        return Results.NotFound();
    }

    contactos.Remove(contacto);
    return Results.NoContent();
});

app.Run();

record Contacto(
    int Id,
    string Nombre,
    string Email,
    string Tel
);

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
