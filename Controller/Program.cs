using Controller.Structs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
InitConfig config = new InitConfig();
DatabaseConnection databaseConnection = JsonSerializer.Deserialize<DatabaseConnection>(config.DatabaseConfigPath);

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
