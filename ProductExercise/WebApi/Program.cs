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

/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApp", policy =>
    {
        policy.WithOrigins("https://product-front-prod.vercel.app", "http://localhost:5196")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
*/

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
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

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();