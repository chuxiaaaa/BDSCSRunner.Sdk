using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 游标滑块
    /// </summary>
    public class Slider : CustomElement
    {
        public Slider()
        {
            Type = "slider";
        }
        /// <summary>
        /// 最小值
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// 游标步值
        /// </summary>
        public float Step { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [JsonProperty("default")] public float Value { get; set; }
    }
}
