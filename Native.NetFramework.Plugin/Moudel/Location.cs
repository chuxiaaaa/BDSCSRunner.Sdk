using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Moudel
{
    public class Postion
    {
        /// <summary>
        /// X轴
        /// </summary>
        public float x { get; set; }
        /// <summary>
        /// Y轴
        /// </summary>
        public float y { get; set; }
        /// <summary>
        /// Z轴
        /// </summary>
        public float z { get; set; }
        /// <summary>
        /// 坐标维度
        /// </summary>
        public int dimensionId { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("x:{0},y:{1},z:{2}", x, y, z);
        }

        public static bool operator ==(Postion p1, Postion p2)
        {
            if (object.Equals(p1, null) || object.Equals(p2, null))
            {
                return object.Equals(p1, p2);
            }
            return p1.x == p2.x && p1.y == p2.y && p1.z == p2.z;
        }

        public static bool operator !=(Postion p1, Postion p2)
        {
            if (object.Equals(p1, null) || object.Equals(p2, null))
            {
                return !object.Equals(p1, p2);
            }
            return !(p1.x == p2.x && p1.y == p2.y && p1.z == p2.z);
        }
    }
}
