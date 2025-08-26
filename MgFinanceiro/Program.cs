using FluentValidation;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Application.Validators;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Infrastructure.Data;
using MgFinanceiro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();

    // Deixa os Enums mais amigavel
    opt.SchemaGeneratorOptions = new SchemaGeneratorOptions
    {
        UseInlineDefinitionsForEnums = true
    };

    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MgFinanceiro - API",
        Version = "1.0",
        Description = "API para gerenciamento financeiro",
        Contact = new OpenApiContact
        {
            Name = "Julio Gabriel",
            Email = "juliogabriel516@gmail.com"
        }
    });
});


// Validators

// Validator direto no controller
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<CreateCategoriaRequest>, CreateCategoriaRequestValidator>();

builder.Services.AddScoped<IValidator<CreateTransacaoRequest>, CreateTransacaoRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateTransacaoRequest>, UpdateTransacaoRequestValidator>();

// Db
builder.Services.AddDbContext<AppDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// DI
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

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