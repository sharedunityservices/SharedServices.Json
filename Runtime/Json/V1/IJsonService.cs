using System.Collections.Generic;
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
            TypeNameHandling = TypeNameHandling.None,
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
        
        public static Dictionary<string, object> ToDictionary(object obj)
        { 
            return ToDictionaryListOrValue(obj) as Dictionary<string, object>;
        }
        
        private static object ToDictionaryListOrValue(object obj)
        {
            var jToken = JToken.FromObject(obj);
            switch (jToken.Type)
            {
                case JTokenType.Object:
                    var dictionary = new Dictionary<string, object>();
                    foreach (var property in jToken.Children<JProperty>()) 
                        dictionary.Add(property.Name, ToDictionaryListOrValue(property.Value));
                    return dictionary;
                
                case JTokenType.Array:
                    var list = new List<object>();
                    foreach (var value in jToken.Children()) 
                        list.Add(ToDictionary(value));
                    return list;
                
                default:
                    return ((JValue) jToken).Value;
            }
        }
    }
}