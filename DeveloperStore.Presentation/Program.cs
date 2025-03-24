using DeveloperStore.Infra.Data.Contexts;
using DeveloperStore.Presentation.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using DeveloperStore.Presentation.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Configura��es de CORS
CorsConfiguration.AddCors(builder);

// Configura��es de servi�os do DI
DependencyInjectionConfiguration.AddDependencyInjection(builder);

// Configura��o do DbContext
//que using eu devo usar pra esse UseSqlServer e GetConnectionString?
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conexao")));

// Adiciona os controladores e as views
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configura��o do JWT
builder.Services.AddJwtConfiguration(builder.Configuration); // Adicionando a configura��o do JWT

// Outras configura��es, como AutoMapper, MediatR, etc
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Especificando o assembly do MediatR (mais seguro do que registrar todos os assemblies)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configura��o de sess�o
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".DeveloperStore.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expira��o conforme necess�rio
    options.Cookie.IsEssential = true; // Para garantir que a sess�o funcione sem consentimento
});

// Cria��o da aplica��o
var app = builder.Build();

// Adicionando middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configura��o para servir arquivos da pasta Uploads
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/uploads" // URL base para acessar os arquivos
});

// Adicionando o middleware de sess�o
app.UseSession();

// Adicionando autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Adicionando o middleware de roteamento
app.UseRouting();

// Configura��o de rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

// Inicia a aplica��o
app.Run();
