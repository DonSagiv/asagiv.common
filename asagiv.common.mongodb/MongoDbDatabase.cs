using asagiv.common.databases;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace asagiv.common.mongodb
{
    public class MongoDbDatabase : IDbDatabase
    {
        #region Fields
        public IMongoDatabase? MongoDatabase { get; }
        public string DatabaseName { get; }
        public IDbClient Client { get; }
        #endregion

        #region Constructor
        public MongoDbDatabase(IDbClient client, IConfiguration configuration)
        {
            if (client is not MongoDbClient mongoDbClient)
            {
                throw new ArgumentException("Incorrect IDbClient type. Expected type is MongoDbClient");
            }

            if(mongoDbClient.MongoClient is null)
            {
                throw new ArgumentException("MongoClient is not connected.");
            }

            var databaseName = configuration["MongoDatabase"];

            Client = client;
            DatabaseName = databaseName;

            MongoDatabase = mongoDbClient.MongoClient?.GetDatabase(databaseName);
        }
        #endregion
    }
}
