using Clinica.Api.Middlewares;
using Clinica.Application.Services;
using Clinica.Application.Services.Impl;   // <- precisa desse para enxergar PessoaService
using Clinica.Domain.Repositories;
using Clinica.Infrastructure.Data;
using Clinica.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))));

// Repositórios
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();

// Serviços (isso resolve seu erro)
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// opcional: não habilite DeveloperExceptionPage; o middleware cuida de tudo
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// ? Nosso formatador de erros
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.Run();

var cs = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 30))));