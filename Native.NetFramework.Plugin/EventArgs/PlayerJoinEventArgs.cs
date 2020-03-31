using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.EventArgs
{
    public class PlayerJoinEventArgs : PluginEventArgs
    {
        public PlayerJoinEventArgs(IPlugin p) : base(p)
        {
        }

        /// <summary>
        /// 玩家
        /// </summary>
        public Moudel.Player p { get; set; }

    }
}
