using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.EventArgs
{
    public class PlayerInputCommandEventArgs : PluginEventArgs
    {
        public PlayerInputCommandEventArgs(IPlugin p) : base(p)
        {
        }

        /// <summary>
        /// 玩家
        /// </summary>
        public Player p { get; set; }
        /// <summary>
        /// 命令
        /// </summary>
        public string cmd { get; set; }
    }
}
