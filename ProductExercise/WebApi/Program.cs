using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<DBContext>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("DatabaseProductExercise")
    )
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:5196")
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
app.UseHttpsRedirection();
app.UseCors("AllowApp");

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();