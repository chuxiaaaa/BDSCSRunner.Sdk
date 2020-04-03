using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.EventArgs
{
    public class PlayerMoveEventArgs : PluginEventArgs
    {
        public PlayerMoveEventArgs(IPlugin p) : base(p)
        {
        }

        public Player p { get; set; }
    }
}
