using DeveloperStore.Application.Handlers;
using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Domain.Interfaces.Services;
using DeveloperStore.Domain.Services;
using DeveloperStore.Infra.Data.Contexts;
using DeveloperStore.Infra.Data.MongoDB.Cache;
using DeveloperStore.Infra.Data.MongoDB.Contexts;
using DeveloperStore.Infra.Data.MongoDB.Settings;
using DeveloperStore.Infra.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DeveloperStore.Presentation.Configuration
{
    public class DependencyInjectionConfiguration
    {
        public static void AddDependencyInjection(WebApplicationBuilder builder)
        {
            // MongoDB configuration
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings")
            );

           builder.Services.AddScoped<MongoDbContext>();

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserAppService, UserAppService>();
            builder.Services.AddScoped<IProductAppService, ProductAppService>();
            builder.Services.AddScoped<ISaleItemAppService, SaleItemAppService>();
            builder.Services.AddScoped<ISaleAppService, SaleAppService>();

            builder.Services.AddScoped<IUserDomainService, UserDomainService>();
            builder.Services.AddScoped<IProductDomainService, ProductDomainService>();
            builder.Services.AddScoped<ISaleItemDomainService, SaleItemDomainService>();
            builder.Services.AddScoped<ISaleDomainService, SaleDomainService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();
            builder.Services.AddScoped<ISaleRepository, SaleRepository>();

            builder.Services.AddTransient<IRequestHandler<UserCreateCommand, UserQuery>, UserRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<UserDeleteCommand, UserQuery>, UserRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<UserPreUpdatePasswordCommand, UserQuery>, UserRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<UserUpdateCommand, UserQuery>, UserRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<UserUpdatePasswordCommand, UserQuery>, UserRequestHandler>();

            builder.Services.AddTransient<IRequestHandler<ProductCreateCommand, ProductQuery>, ProductRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<ProductDeleteCommand, ProductQuery>, ProductRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<ProductUpdateCommand, ProductQuery>, ProductRequestHandler>();

            builder.Services.AddTransient<IRequestHandler<SaleItemCreateCommand, SaleItemQuery>, SaleItemRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<SaleItemDeleteCommand, SaleItemQuery>, SaleItemRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<SaleItemUpdateCommand, SaleItemQuery>, SaleItemRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<SaleItemCalculateTotalItemAmountCommand, SaleItemQuery>, SaleItemRequestHandler>();

            builder.Services.AddTransient<IRequestHandler<SaleCreateCommand, SaleQuery>, SaleRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<SaleDeleteCommand, SaleQuery>, SaleRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<SaleUpdateCommand, SaleQuery>, SaleRequestHandler>();
            builder.Services.AddTransient<IRequestHandler<SaleItemSaleCompleteCommand, SaleQuery>, SaleRequestHandler>();
          

            // Cache (MongoDB)
            builder.Services.AddScoped<IUserCache, UserCache>();
            builder.Services.AddScoped<IProductCache, ProductCache>();
            builder.Services.AddScoped<ISaleItemCache, SaleItemCache>();
            builder.Services.AddScoped<ISaleCache, SaleCache>();

            // Unit of Work and Contexts
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<DataContext>();
        }


    }
}
