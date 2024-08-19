using Newtonsoft.Json;

namespace SharedServices.Json.V1
{
    [UnityEngine.Scripting.Preserve]
    public class JsonService : IJsonService
    {
        public string ToJson<T>(T obj, JsonSerializerSettings settings = null) => 
            IJsonService.ToJson(obj, settings);

        public T FromJson<T>(string json, JsonSerializerSettings settings = null) => 
            IJsonService.FromJson<T>(json, settings);
    }
}