using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 模式对话框
    /// </summary>
    public class ModalForm : Form
    {
        public string Content { get; set; }
        public string Button1 { get; set; }
        public string Button2 { get; set; }
        [JsonIgnore]
        public ButtonClickEvent button1ClickEvent;
        [JsonIgnore]
        public ButtonClickEvent button2ClickEvent;

        public ModalForm()
        {
            Type = "modal";
        }
    }
}
