using Microsoft.EntityFrameworkCore;
using RustdeskServerAPI;

var builder = WebApplication.CreateBuilder(args);

string connection = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=somePassword984";
builder.Services.AddDbContext<ApiContext>(options => options.UseNpgsql(connection));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    //var response = context.Request;
    await next.Invoke();
    
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
