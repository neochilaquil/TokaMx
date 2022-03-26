using Microsoft.EntityFrameworkCore;
using TEST_DEV_SHN_24_Marzo_2022.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStrng = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PersonasContext>(opt => opt.UseSqlServer(connectionStrng));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
