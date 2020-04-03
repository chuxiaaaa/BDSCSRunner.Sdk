using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.EventArgs
{
    /// <summary>
    /// 爆炸事件参数
    /// </summary>
    public class LevelExploedEventArgs : PluginEventArgs
    {
        public LevelExploedEventArgs(IPlugin p) : base(p)
        {
        }

        public Postion postion { get; set; }

        public string entity { get; set; }

        public int entityId { get; set; }

        public int dimensionId { get; set; }

        public int explodePower { get; set; }
    }
}
