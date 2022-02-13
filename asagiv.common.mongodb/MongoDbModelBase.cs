using asagiv.common.databases;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbModelBase : IDbModel<ObjectId>
    {
        #region Properties
        [BsonId]
        public ObjectId Id { get; set; }
        #endregion

        #region Constructor
        public MongoDbModelBase()
        {
            Id = new ObjectId();
        }
        #endregion
    }
}