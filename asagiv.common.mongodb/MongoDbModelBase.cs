using asagiv.common.databases;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace asagiv.common.mongodb
{
    public abstract class MongoDbModelBase : IDbModel<ObjectId>
    {
        #region Properties
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonExtraElements]
        public IDictionary<string, object> ExtraElements { get; set; }
        #endregion

        #region Constructor
        protected MongoDbModelBase()
        {
            Id = ObjectId.GenerateNewId();
            ExtraElements = new Dictionary<string, object>();
        }
        #endregion
    }
}