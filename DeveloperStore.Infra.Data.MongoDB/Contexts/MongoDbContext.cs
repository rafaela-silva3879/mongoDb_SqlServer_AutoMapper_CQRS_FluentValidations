using DeveloperStore.Domain.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using DeveloperStore.Infra.Data.MongoDB.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DeveloperStore.Infra.Data.MongoDB.Contexts
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var settings = mongoDbSettings.Value;

            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.Host));

            if (settings.isSSL)
            {
                clientSettings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };
            }

            var mongoClient = new MongoClient(clientSettings); 
            _mongoDatabase = mongoClient.GetDatabase(settings.Name);

            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id).SetIdGenerator(null);
                });
            }
        }

        // Expondo as coleções do banco
        public IMongoCollection<User> Users =>
            _mongoDatabase.GetCollection<User>("users");

        public IMongoCollection<Product> Products =>
            _mongoDatabase.GetCollection<Product>("products");

        public IMongoCollection<SaleItem> SaleItems =>
            _mongoDatabase.GetCollection<SaleItem>("saleItems");

        public IMongoCollection<Sale> Sales =>
            _mongoDatabase.GetCollection<Sale>("sales");
    }
}
