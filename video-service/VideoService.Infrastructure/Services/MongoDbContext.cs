using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VideoService.Application.Context;
using VideoService.Domain.Models;
using VideoService.Infrastructure.Settings;

namespace VideoService.Infrastructure.Services
{
    public class MongoDbContext : IMongoDbContext
    {
        public readonly IMongoDatabase database;

        public MongoDbContext(IOptions<MongoDbSetting> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>() where T : BaseEntity
        {
            return database.GetCollection<T>(typeof(T).Name);
        }
    }
}
