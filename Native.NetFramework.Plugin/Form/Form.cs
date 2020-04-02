using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    public class Form
    {
        /// <summary>
        /// 窗体类型
        /// </summary>
        public string Type { get; protected set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        [JsonIgnore]
        /// <summary>
        /// 玩家点击关闭事件
        /// </summary>
        public Action FormExitEvent { get; set; }

        public string ToJson()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Error;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var content = JsonConvert.SerializeObject(this, settings);
            return content;
        }

    }
}
