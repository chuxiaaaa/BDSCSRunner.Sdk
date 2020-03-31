using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 开关
    /// </summary>
    public class Toggle : CustomElement
    {
        public Toggle()
        {
            Type = "toggle";
        }
        /// <summary>
        /// 默认值
        /// </summary>
        [JsonProperty("default")]
        public bool Value { get; set; }
    }
}
