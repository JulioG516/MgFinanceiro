using FluentValidation;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Application.Validators;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Infrastructure.Data;
using MgFinanceiro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Validators

// Validator direto no controller
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<CreateCategoriaRequest>, CreateCategoriaRequestValidator>();

// Db
builder.Services.AddDbContext<AppDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// DI
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

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