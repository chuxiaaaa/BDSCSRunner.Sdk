using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    public class CustomForm : Form
    {
        public CustomForm()
        {
            Type = "custom_form";
        }
        /// <summary>
        /// 自定义元素列表
        /// </summary>
        public List<CustomElement> Content { get; set; }
        [JsonIgnore]
        public FormSubmitEvent submitEvent { get; set; }

        public delegate void FormSubmitEvent(object[] result);

    }
}
