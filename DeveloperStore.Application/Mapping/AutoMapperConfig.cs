using AutoMapper;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Entities.Enums;

namespace DeveloperStore.Application.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Mapping from Product to UserQuery
            CreateMap<User, UserQuery>()
                .AfterMap((entity, query) =>
                {
                    query.Id = entity.Id;
                    query.Name = entity.Name;
                    query.UserProfile = entity.UserProfile;
                    query.Email = entity.Email;
                    query.PasswordResetToken = entity.PasswordResetToken;
                    query.PasswordResetExpiresIn = entity.PasswordResetExpiresIn;
                });

            // Mapping from ProductCreateCommand to Product
            CreateMap<UserCreateCommand, User>()
                .AfterMap((source, destination) =>
                {
                    destination.UserProfile = (int)UserProfile.User;
                });

            // Additional specific mapping for ProductQuery based on email and reset token
            CreateMap<User, UserQuery>()
                .AfterMap((entity, query) =>
                {
                    query.Email = entity.Email;
                    query.PasswordResetToken = entity.PasswordResetToken;
                    query.PasswordResetExpiresIn = entity.PasswordResetExpiresIn;
                });

            // Mapping for ProductGetProductByTokenResponse
            CreateMap<User, UserQuery>()
                .AfterMap((entity, query) =>
                {
                    query.Id = entity.Id;
                    query.Name = entity.Name;
                    query.Email = entity.Email;
                    query.PasswordResetToken = entity.PasswordResetToken;
                    query.PasswordResetExpiresIn = entity.PasswordResetExpiresIn;
                    query.UserProfile = entity.UserProfile;
                });

            //Mapping for GetByIdAsync
            CreateMap<Product, ProductQuery>();
            CreateMap<Sale, SaleQuery>();



            CreateMap<ProductCreateCommand, Product>(); // Firstly, map Product
            CreateMap<Product, ProductQuery>(); // Afterwords, map Product to ProductQuery

            CreateMap<SaleItemCreateCommand, SaleItem>();
            CreateMap<SaleItem, SaleItemQuery>();

            CreateMap<Sale, SaleQuery>()
               .AfterMap((entity, query) =>
               {
                   query.SaleDate = entity.SaleDate;
                   query.IsCancelled = false;
                   query.TotalSaleAmount = entity.TotalSaleAmount;
               });

            CreateMap<SaleItemCalculateTotalItemAmountCommand, SaleItem>()
            .AfterMap((source, destination) =>
            {
                destination.ProductId = source.ProductId;
                destination.Quantity = source.Quantity;
            });
        }
    }
}