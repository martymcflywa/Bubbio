using Bubbio.MongoDb.Interfaces;
using MongoDB.Driver;

namespace Bubbio.MongoDb.Tests
{
    public class TestMongoDbRepository : MongoDbRepository
    {
        public TestMongoDbRepository(IMongoDbContext dbContext)
            : base(dbContext)
        {
        }

        public TestMongoDbRepository(MongoUrl url)
            : base(url)
        {
        }

        public TestMongoDbRepository(IMongoDatabase database)
            : base(database)
        {
        }

        public TestMongoDbRepository(string connectionString, string databaseName)
            : base(connectionString, databaseName)
        {
        }
    }
}