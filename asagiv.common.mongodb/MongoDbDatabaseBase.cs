using asagiv.common.databases;
using MongoDB.Bson;
using MongoDB.Driver;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbDatabaseBase : IDbDatabase
    {
        #region Fields
        public IMongoDatabase? _mongoDatabase;
        #endregion

        #region Properties
        public string DatabaseName { get; }
        public IDbClient Client { get; }
        #endregion

        #region Constructor
        public MongoDbDatabaseBase(MongoDbClientBase client, string databaseName)
        {
            _mongoDatabase = client.GetMongoDatabase(databaseName);

            Client = client;
            DatabaseName = databaseName;
        }
        #endregion

        #region Methods
        public IMongoCollection<TDbModel>? GetMongoView<TDbModel>(string viewName)
            where TDbModel : IDbModel<ObjectId>
        {
            return _mongoDatabase?.GetCollection<TDbModel>(viewName);
        }
        #endregion
    }
}
