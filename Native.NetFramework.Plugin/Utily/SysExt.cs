using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Utily
{
    public static class SysExt
    {
        private static Encoding encoding = Encoding.GetEncoding("utf-8");

        [DllImport("kernel32.dll", EntryPoint = "lstrlenA", CharSet = CharSet.Ansi)]
        public extern static int LstrlenA(IntPtr ptr);

        public static string GetString(this IntPtr strPtr)
        {
            int len = LstrlenA(strPtr);   //获取指针中数据的长度
            if (len == 0)
            {
                return string.Empty;
            }
            byte[] buffer = new byte[len];
            Marshal.Copy(strPtr, buffer, 0, len);
            return encoding.GetString(buffer);
        }

        /// <summary>
        /// 获取当前对象的 <see cref="GCHandle"/> 实例, 该实例为 <see cref="GCHandleType.Pinned"/> 类型
        /// </summary>
        /// <param name="source">将转换的对象</param>
        /// <param name="encoding">转换的编码</param>
        /// <returns></returns>
        public static GCHandle GetGCHandle(this string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = SysExt.encoding;
            }
            byte[] buffer;
            if (source == null)
            {
                buffer = new byte[0];
            }
            else
            {
                buffer = encoding.GetBytes(source);
            }
            return GCHandle.Alloc(buffer, GCHandleType.Pinned);
        }
    }
}
