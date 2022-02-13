using asagiv.common.databases;
using MongoDB.Bson;
using MongoDB.Driver;
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
        public IMongoCollection<TDbModel>? _mongoCollection;
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

            await _mongoCollection.FindOneAndReplaceAsync(filter, modelToAdd);
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

        public async Task<TDbModel?> ReadAsync(ObjectId id)
        {
            if (_mongoCollection == null)
            {
                return null;
            }

            var filter = Builders<TDbModel>.Filter.Where(x => x.Id == id);

            return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async IAsyncEnumerable<TDbModel?> ReadManyAsync(IEnumerable<ObjectId> idList, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (_mongoCollection == null)
            {
                yield break;
            }

            var filter = Builders<TDbModel>.Filter.In(x => x.Id, idList);

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
        #endregion
    }
}
