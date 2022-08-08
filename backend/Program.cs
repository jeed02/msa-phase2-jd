using backend.Data;
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
builder.Services.AddScoped<ICharacterRepo, CharacterService>();

if (builder.Environment.IsDevelopment()){
    builder.Services.AddDbContext<CharacterDb>(options => options.UseInMemoryDatabase("Characters"));
} else if (builder.Environment.IsProduction()){
    builder.Services.AddDbContext<CharacterDb>(options => options.UseSqlite(builder.Configuration["GenshinAPIConnection"]));
}

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
