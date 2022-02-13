using asagiv.common.databases;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbEntityBase<TDbModel> : IDbEntity<TDbModel, ObjectId>
        where TDbModel : class, IDbModel<ObjectId>
    {
        #region Fields
        private readonly Task<TDbModel?>? _retrieveTask;
        #endregion

        #region Properties
        public string? Label { get; set; }
        public ObjectId Id { get; }
        #endregion

        #region Constructor
        public MongoDbEntityBase(MongoDbCollectionBase<TDbModel> collection, ObjectId id, string label = "")
        {
            Id = id;
            Label = label;

            if(collection != null)
            {
                _retrieveTask = collection.ReadAsync(id);
            }
        }
        #endregion

        #region Methods
        public  async Task<TDbModel?> GetModelAsync()
        {
            if(_retrieveTask == null)
            {
                return null;
            }

            var retrievedModel = await _retrieveTask;

            if(retrievedModel == null)
            {
                return null;
            }

            return await GetModelOverrideAsync(retrievedModel);
        }

        protected virtual Task<TDbModel> GetModelOverrideAsync(TDbModel retrievedModel)
        {
            return Task.FromResult(retrievedModel);
        }
        #endregion
    }
}
