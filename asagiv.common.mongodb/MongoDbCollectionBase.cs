using asagiv.common.databases;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbCollectionBase<TDbModel> : IDbCollection<TDbModel, ObjectId>
        where TDbModel : class, IDbModel<ObjectId>
    {
        #region Fields
        private readonly IMongoCollection<TDbModel>? _mongoCollection;
        #endregion

        #region Properties
        public IDbDatabase Database { get; }
        public string ViewName { get; }
        #endregion

        #region Constructor
        protected MongoDbCollectionBase(MongoDbDatabaseBase mongoDbDatabase, string viewName)
        {
            _mongoCollection = mongoDbDatabase.GetMongoView<TDbModel>(viewName);

            ViewName = viewName;
            Database = mongoDbDatabase;
        }
        #endregion

        #region Methods
        public async Task AppendAsync(TDbModel modelToAdd)
        {
            if (_mongoCollection == null)
            {
                return;
            }

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == modelToAdd.Id);

            var options = new FindOneAndReplaceOptions<TDbModel> { IsUpsert = true };

            await _mongoCollection.FindOneAndReplaceAsync(filter, modelToAdd, options);
        }

        public async Task<TDbModel?> DeleteAsync(ObjectId id)
        {
            if (_mongoCollection == null)
            {
                return null;
            }

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == id);

            var deletedData = await _mongoCollection.FindOneAndDeleteAsync(filter);

            return deletedData;
        }

        public async Task DeleteManyAsync(params ObjectId[] idList)
        {
            if (_mongoCollection == null)
            {
                return;
            }

            var filter = Builders<TDbModel>.Filter.In(x => x.Id, idList);

            await _mongoCollection.DeleteManyAsync(filter);
        }

        public async Task<TDbModel?> ReadAsync(ObjectId id)
        {
            if (_mongoCollection == null)
            {
                return null;
            }

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == id);

            return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDbModel?>?> ReadManyAsync(IEnumerable<ObjectId>? idList = null)
        {
            if (_mongoCollection == null)
            {
                return null;
            }

            var filter = GetReadManyFilter(idList);

            return await _mongoCollection.Find(filter).ToListAsync();
        }

        public async IAsyncEnumerable<TDbModel?> GetEnumerable(IEnumerable<ObjectId>? idList = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (_mongoCollection == null)
            {
                yield break;
            }

            var filter = GetReadManyFilter(idList);

            var cursor = await _mongoCollection.Find(filter).ToCursorAsync(cancellationToken);

            while (await cursor.MoveNextAsync(cancellationToken))
            {
                var currentBatch = cursor.Current;

                foreach (var item in currentBatch)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    yield return item;
                }
            }
        }

        public IMongoQueryable<TDbModel> AsQueryable()
        {
            return _mongoCollection.AsQueryable();
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
