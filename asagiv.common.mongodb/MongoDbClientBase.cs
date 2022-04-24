using asagiv.common.databases;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbClientBase : IDbClient
    {
        #region Fields
        private MongoClient? _client;
        #endregion

        #region Methods
        public void Connect(string connectionString)
        {
            _client = new MongoClient(connectionString);
        }

        public Task ConnectAsync(string connectionString)
        {
            Connect(connectionString);

            return Task.CompletedTask;
        }

        public IMongoDatabase? GetMongoDatabase(string databaseName)
        {
            return _client?.GetDatabase(databaseName);
        }
        #endregion
    }
}
