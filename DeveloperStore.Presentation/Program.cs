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

// Configurações de CORS
CorsConfiguration.AddCors(builder);

// Configurações de serviços do DI
DependencyInjectionConfiguration.AddDependencyInjection(builder);

// Configuração do DbContext
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

// Configuração do JWT
builder.Services.AddJwtConfiguration(builder.Configuration); // Adicionando a configuração do JWT

// Outras configurações, como AutoMapper, MediatR, etc
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Especificando o assembly do MediatR (mais seguro do que registrar todos os assemblies)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configuração de sessão
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".DeveloperStore.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expiração conforme necessário
    options.Cookie.IsEssential = true; // Para garantir que a sessão funcione sem consentimento
});

// Criação da aplicação
var app = builder.Build();

// Adicionando middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configuração para servir arquivos da pasta Uploads
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/uploads" // URL base para acessar os arquivos
});

// Adicionando o middleware de sessão
app.UseSession();

// Adicionando autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Adicionando o middleware de roteamento
app.UseRouting();

// Configuração de rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

// Inicia a aplicação
app.Run();
