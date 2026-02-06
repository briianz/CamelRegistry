using CamelRegistry.Data;
using CamelRegistry.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Swagger regisztrálása
builder.Services.AddSwaggerGen();

//Adatbázis (SQLite) regisztrálása
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=camels.db"));

//CORS beállítása, hogy a frontend alkalmazásunk is hozzáférhessen az API-hoz
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

//Swagger használata, ha a környezet fejlesztői környezet
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Új teve létrehozása (POST)
app.MapPost("/api/camels", async (Camel camel, AppDbContext db) =>
{

    if (camel.HumpCount < 1 || camel.HumpCount > 2) // Validáció a púpok számára
    {
        return Results.BadRequest("A tevének csak 1 vagy 2 púpja lehet.");
    }

    db.Camels.Add(camel);

    await db.SaveChangesAsync();

    return Results.Created($"/api/camels/{camel.Id}", camel);

});

//Tevék listázása (GET)
app.MapGet("/api/camels", async (AppDbContext db) =>
{
    var camels = await db.Camels.ToListAsync();

    return Results.Ok(camels);
});

//Egy adott teve lekérdezése (GET/{id})
app.MapGet("/api/camels/{id}", async (int id, AppDbContext db) =>
{
    var camel = await db.Camels.FindAsync(id);

    if (camel == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(camel);

});

/*Egy adott teve adatainak módosítása (PUT/{id})
    Az összes adatot meg kell adni, és az adott id-n már meglévő teve adatait teljesen felülírja*/
app.MapPut("/api/camels/{id}", async (int id, Camel updatedCamel, AppDbContext db) =>
{
    if (updatedCamel.HumpCount < 1 || updatedCamel.HumpCount > 2) // Validáció a púpok számára itt is 
    {
        return Results.BadRequest("A tevének csak 1 vagy 2 púpja lehet.");
    }
    var camel = await db.Camels.FindAsync(id);
    if (camel == null) //Ha az adott id-n nincs teve, akkor hibát dobunk vissza
    {
        return Results.NotFound();
    }
    camel.Name = updatedCamel.Name;
    camel.Color = updatedCamel.Color;
    camel.HumpCount = updatedCamel.HumpCount;
    camel.LastFed = updatedCamel.LastFed;
    await db.SaveChangesAsync();
    return Results.Ok(camel);
});

//Teve törlése (DELETE/{id})
app.MapDelete("/api/camels/{id}", async (int id, AppDbContext db) =>
{
    var camel = await db.Camels.FindAsync(id);
    if (camel == null)
    {
        return Results.NotFound();
    }
    db.Camels.Remove(camel);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseCors();
app.Run();

//Megnyitjuk a program osztályt, hogy a tesztekben is elérhető legyen
public partial class Program { }


