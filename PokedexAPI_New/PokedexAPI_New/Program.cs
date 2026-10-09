using Microsoft.EntityFrameworkCore;
using PokedexAPI_New.Data;
using PokedexAPI_New.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PokedexContext>(options =>
    options.UseSqlite("Data Source=pokedex.db"));

// Allow frontend to call the API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

// Seed database once
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PokedexContext>();
    db.Database.EnsureCreated();
    PokeApiImporter.ImportPokemons(db, 151).Wait();
}

app.Run();