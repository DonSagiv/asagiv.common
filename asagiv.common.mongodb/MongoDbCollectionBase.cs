using asagiv.common.databases;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System;
using Serilog;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbCollectionBase<TDbModel> : IDbCollection<TDbModel, ObjectId>
        where TDbModel : class, IDbModel<ObjectId>
    {
        #region Fields
        private readonly ILogger? _logger;
        #endregion

        #region Properties
        public IMongoCollection<TDbModel>? MongoCollection { get; }
        public IDbDatabase Database { get; }
        public string ViewName { get; }
        #endregion

        #region Constructor
        protected MongoDbCollectionBase(IDbDatabase database, string viewName, ILogger? logger = null)
        {
            if(database is not MongoDbDatabase mongoDbDatabase)
            {
                throw new ArgumentException("Incorrect IDbDatabase type. Expected type is MongoDbDatabase.");
            }

            if (mongoDbDatabase.MongoDatabase is null)
            {
                throw new ArgumentException("MongoDatabase is not available.");
            }

            _logger = logger;

            ViewName = viewName;
            Database = database;

            _logger?.Information("Initializing MongoDB Collection {ViewName} in {Database}", ViewName, Database.DatabaseName);

            MongoCollection = mongoDbDatabase.MongoDatabase?.GetCollection<TDbModel>(viewName);
        }
        #endregion

        #region Methods
        public async Task AppendAsync(TDbModel modelToAdd)
        {
            if (MongoCollection == null)
            {
                throw new NullReferenceException("MongoCollection reference not found.");
            }

            _logger?.Information($"Appending {modelToAdd.Id} from {ViewName}");

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == modelToAdd.Id);

            var options = new FindOneAndReplaceOptions<TDbModel> { IsUpsert = true };

            await MongoCollection.FindOneAndReplaceAsync(filter, modelToAdd, options);
        }

        public async Task<TDbModel?> DeleteAsync(ObjectId id)
        {
            _logger?.Information("Deleting {Id} from {ViewName}.", id, ViewName);

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == id);

            var deletedData = await MongoCollection.FindOneAndDeleteAsync(filter);

            return deletedData;
        }

        public async Task DeleteManyAsync(params ObjectId[] idList)
        {
            if (idList.Length == 0)
            {
                return;
            }

            var loggerString = string.Join(", ", idList);

            _logger?.Information("Deleting {LoggerString} from {ViewName}.", loggerString, ViewName);

            var filter = Builders<TDbModel>.Filter.In(x => x.Id, idList);

            var result = await MongoCollection.DeleteManyAsync(filter);
        }

        public async Task<TDbModel?> ReadAsync(ObjectId id)
        {
            _logger?.Information("Retrieving {Id} from {ViewName}.", id, ViewName);

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == id);

            return await MongoCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDbModel?>?> ReadManyAsync(IEnumerable<ObjectId>? idList = null)
        {
            if(idList is null)
            {
                _logger?.Information("Retrieving all items from {ViewName}.", ViewName);
            }
            else
            {
                var loggerString = string.Join(", ", idList);

                _logger?.Information("Retrieving {loggerString} from {ViewName}.", loggerString, ViewName);
            }

            var filter = GetReadManyFilter(idList);

            return await MongoCollection.Find(filter).ToListAsync();
        }

        public async IAsyncEnumerable<TDbModel?> GetEnumerable(IEnumerable<ObjectId>? idList = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.Information("Retrieving Enumerable from {ViewName}.", ViewName);

            var filter = GetReadManyFilter(idList);

            var cursor = await MongoCollection.Find(filter).ToCursorAsync(cancellationToken);

            while (await cursor.MoveNextAsync(cancellationToken))
            {
                var currentBatch = cursor.Current;

                _logger?.Debug("Retrieved {CursorCount} items in batch from {ViewName}.", cursor.Current.Count(), ViewName);

                foreach (var item in currentBatch)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _logger?.Information("Enumerable Retrieval from {ViewName}: Cancellation Invoked.", ViewName);

                        yield break;
                    }

                    _logger?.Debug("Retrieved {Id} from {ViewName}.", item.Id, ViewName);

                    yield return item;
                }
            }
        }

        public IMongoQueryable<TDbModel> AsQueryable()
        {
            _logger?.Information("Retrieving {ViewName} Queryable.", ViewName);

            return MongoCollection.AsQueryable();
        }

        private static FilterDefinition<TDbModel> GetReadManyFilter(IEnumerable<ObjectId>? idList)
        {
            return idList == null
                ? Builders<TDbModel>.Filter.Exists(x => x.Id)
                : Builders<TDbModel>.Filter.In(x => x.Id, idList);
        }
        #endregion
    }
}
