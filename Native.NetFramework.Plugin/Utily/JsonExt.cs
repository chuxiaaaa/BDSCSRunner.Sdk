using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Utily
{
    public static class JsonExt
    {
        public static JObject ParseJsonJObject(this string json)
        {
            return JObject.Parse(json);
        }

        public static T ParseJsonT<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
        }
    }
}
