using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 矩阵滑块
    /// </summary>
    public class StepSlider : CustomElement
    {
        public StepSlider()
        {
            Type = "step_slider";
        }

        /// <summary>
        /// 默认值
        /// </summary>
        [JsonProperty("default")] public int Value { get; set; }
        /// <summary>
        /// 矩阵数组
        /// </summary>
        public List<string> Steps { get; set; } = new List<string>();
    }
}
