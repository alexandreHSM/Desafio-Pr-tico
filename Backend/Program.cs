using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; 

var builder = WebApplication.CreateBuilder(args);

//Configura o Banco de Dados SQLite
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<TransacaoService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // ajuste pra porta que seu React vai usar
              .AllowAnyHeader()   // permite qualquer header (ex: Content-Type: application/json)
              .AllowAnyMethod();  // permite GET, POST, DELETE, etc.
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("PermitirReact");

app.MapControllers();

// Ela mapeia as rotas como /api/pessoas
app.MapControllers(); 

// GARANTE A CRIAÇÃO AUTOMÁTICA DAS TABELAS NO INÍCIO
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Cria o banco e as tabelas se não existirem
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrações.");
    }
}


app.Run();

