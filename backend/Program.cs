using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TreinamentoAPI.Data;
using TreinamentoAPI.Models;
using TreinamentoAPI.Services;
using TreinamentoAPI.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Logging
builder.Logging.AddConsole();

// Adicionar DbContext ao container de DI
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração centralizada via IOptions<T>
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Registrar Services e Repositories com diferentes lifetimes
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// Configurar AutoMapper para mapeamento de Entities em DTOs
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adicionar Controllers
builder.Services.AddControllers();

// Configurar CORS para aceitar requisições de origem específica
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger/OpenAPI para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar política de CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();