using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.EventArgs
{
    /// <summary>
    /// 玩家提交GUI事件参数
    /// </summary>
    public class PlayerFormSelectEventArgs : PluginEventArgs
    {
        public PlayerFormSelectEventArgs(IPlugin p) : base(p)
        {
        }

        public Player p { get; set; }

        public int formId { get; set; }

        public string selected { get; set; }
    }
}
