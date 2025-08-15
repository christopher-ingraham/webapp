using Newtonsoft.Json;

namespace DA.WI.NSGHSM.Core.Extensions
{
    public static class JsonExtensions
    {
        public static T JsonClone<T>(this T me)
        {
            if (me == null)
                return default(T);

            // To serialize/deserialize polymorphic classes
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            return me
                .ToJson(settings)
                .FromJson<T>(settings);
        }

        public static string ToJson(this object me, JsonSerializerSettings settings = null)
        {
            if (me == null)
                return null;

            return settings != null
                ? JsonConvert.SerializeObject(me, settings)
                : JsonConvert.SerializeObject(me);
        }

        public static T FromJson<T>(this string me, JsonSerializerSettings settings = null)
        {
            if (me == null)
                return default(T);

            return settings != null
                ? JsonConvert.DeserializeObject<T>(me, settings)
                : JsonConvert.DeserializeObject<T>(me);
        }
    }
}
