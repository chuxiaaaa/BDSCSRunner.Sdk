using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    /// <summary>
    /// 简易表单
    /// </summary>
    public class SimpleForm : Form
    {
        public SimpleForm()
        {
            Type = "form";
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 按钮列表
        /// </summary>
        public List<Button> Buttons { get; set; } = new List<Button>();
    }
}
