using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace CustomerManagementApp.Serialization
{
    public class GuidSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            return type == typeof(Guid) ? new GuidSerializer(GuidRepresentation.Standard) : null;
        }
    }
}
