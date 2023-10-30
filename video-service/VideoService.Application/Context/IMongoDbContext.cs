using MongoDB.Driver;
using VideoService.Domain.Models;

namespace VideoService.Application.Context;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>() where T : BaseEntity;
}
