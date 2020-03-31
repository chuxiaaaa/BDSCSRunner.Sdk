using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 文本框
    /// </summary>
    public class Input : CustomElement
    {
        public Input()
        {
            Type = "input";
        }
        /// <summary>
        /// 占位符
        /// </summary>
        public string Placeholder { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [JsonProperty("default")] 
        public string Value { get; set; }
    }
}
