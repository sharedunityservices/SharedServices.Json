using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedServices.Log;
using SharedServices.V1;

namespace SharedServices.Json.V1
{
    public interface IJsonService : IService
    {
        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            Formatting = Formatting.Indented
        }; 
        
        public static string ToJson<T>(T obj, JsonSerializerSettings settings = null)
        {
            ILog.Trace($"Serializing {typeof(T).Name} to JSON", obj);
            var json = JsonConvert.SerializeObject(obj, settings ?? DefaultSettings);
            ILog.Trace($"Serialized {typeof(T).Name} to JSON\n{json}", obj);
            return json;
        }
        
        public static T FromJson<T>(string json, JsonSerializerSettings settings = null)
        {
            ILog.Trace($"Deserializing JSON to {typeof(T).Name}\n{json}", typeof(IJsonService));
            var obj = JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSettings);
            ILog.Trace($"Deserialized JSON to {typeof(T).Name}", obj);
            return obj;
        }

        public static JToken FromJson(string json)
        {
            ILog.Trace($"Deserializing JSON to JToken\n{json}", typeof(IJsonService));
            var jToken = JToken.Parse(json);
            ILog.Trace($"Deserialized JSON to JToken", jToken);
            return jToken;
        }
    }
}