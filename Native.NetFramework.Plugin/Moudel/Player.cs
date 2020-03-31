using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Moudel
{
    public class Player
    {
        /// <summary>
        /// 玩家名
        /// </summary>
        public string playerName { get; set; }
        /// <summary>
        /// UUID
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// XUID
        /// </summary>
        public string xuid { get; set; }
        /// <summary>
        /// 玩家坐标
        /// </summary>
        public Postion postion { get; set; }

        /// <summary>
        /// 维度ID
        /// </summary>
        public int dimensionId { get; set; }
        /// <summary>
        /// 是否站立
        /// </summary>
        public int isstand { get; set; }

    }
}
