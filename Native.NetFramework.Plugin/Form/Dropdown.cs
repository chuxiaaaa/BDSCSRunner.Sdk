using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 下拉框
    /// </summary>
    public class Dropdown : CustomElement
    {
        public Dropdown()
        {
            Type = "dropdown";
        }
        /// <summary>
        /// 默认值
        /// </summary>
        [JsonProperty("default")]
        public int Value { get; set; }
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<string> Options { get; set; } = new List<string>();
    }
}
