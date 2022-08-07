using backend.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("genshin", configureClient: client =>
{
    client.BaseAddress = new Uri("https://api.genshin.dev/characters");
});

builder.Services.AddMvc();  
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
if (builder.Environment.IsDevelopment()){
    builder.Services.AddDbContext<CharacterDb>(options => options.UseInMemoryDatabase("items"));
} else if (builder.Environment.IsProduction()){
    var connectionString = builder.Configuration.GetConnectionString("Characters") ?? "Data Source=Characters.db";
    builder.Services.AddDbContext<CharacterDb>(options => options.UseSqlite(connectionString));

}

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
