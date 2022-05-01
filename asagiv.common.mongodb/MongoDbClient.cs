using asagiv.common.databases;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace asagiv.common.mongodb
{
    public sealed class MongoDbClient : IDbClient
    {
        #region Fields
        private readonly string? _connectionString;
        private readonly ILogger? _logger;
        #endregion

        #region Properties
        public IMongoClient? MongoClient { get; private set; }
        #endregion

        #region Constructors
        public MongoDbClient(IConfiguration configuration, ILogger? logger = null)
        {
            _logger = logger;

            var environmentConnectionString = Environment.GetEnvironmentVariable("MONGO_CONN_STR");

            if (!string.IsNullOrWhiteSpace(environmentConnectionString))
            {
                _logger?.Information("Mongo connection environment variable found: {MongoConnStr}", environmentConnectionString);

                Connect(environmentConnectionString);
            }
            else
            {
                _connectionString = configuration.GetConnectionString("MongoDB");

                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    throw new Exception("Could not find connedction string for MongoDB in appsettings.json");
                }

                _logger?.Information("Using appsettings.json connection string: {MongoConnStr}", _connectionString);

                Connect(_connectionString);
            }
        }
        #endregion

        #region Methods
        public void Connect(string connectionString)
        {
            _logger?.Information("Attempting to connect to {ConnectionString}", connectionString);

            try
            {
                MongoClient = new MongoClient(connectionString);

                _logger?.Information("Connection Successful.");
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Unable to connnect to MongoDB via {connectionString}.", connectionString);

                throw;
            }
        }

        public Task ConnectAsync(string connectionString)
        {
            Connect(connectionString);

            return Task.CompletedTask;
        }
        #endregion
    }
}
