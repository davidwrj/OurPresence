using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OurPresence.Core.Money.Extensions
{
    /// <summary>
    /// Extension methods used for Json Serialization
    /// </summary>
    public static class JsonExtensions
    {
        private static JsonSerializerSettings GetSettings(bool includeNull = true)
        {
            return new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
        }

        /// <summary>
        /// Serialize object into a valid Json string using NewtonSoft.Json
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object being serialized</param>
        /// <param name="includeNull">Flag to determine if null properties will be serialized</param>
        /// <returns>Json string representation of the object</returns>
        public static string ToJson<T>(this T obj)            
        {
            return ToJson(obj, true);
        }

        /// <summary>
        /// Serialize object into a valid Json string using NewtonSoft.Json
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="obj">Object being serialized</param>
        /// <param name="includeNull">Flag to determine if null properties will be serialized</param>
        /// <returns>Json string representation of the object</returns>
        public static string ToJson<T>(this T obj, bool includeNull)
        {
            var settings = GetSettings(includeNull);
            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Deserialize a valid json string into it object type
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="json">Json string representing the object</param>
        /// <returns>Object of the type specified</returns>
        public static T? FromJson<T>(this string json)
        {
            ITraceWriter traceWriter = new MemoryTraceWriter();
            var settings = GetSettings();
            settings.TraceWriter = traceWriter;
            
            var data = JsonConvert.DeserializeObject<T>(json, settings);
            Debug.WriteLine(traceWriter);

            return data;
        }
    }
}
