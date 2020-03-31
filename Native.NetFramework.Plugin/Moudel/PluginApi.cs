using Native.NetFramework.Plugin.Utily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Moudel
{
    public class PluginApi
    {
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);
        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);
        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);
        private IntPtr hLib;
        public PluginApi(String DLLPath)
        {
            hLib =  LoadLibrary(DLLPath);
        }

        ~PluginApi()
        {
            FreeLibrary(hLib);
        }
        //将要执行的函数转换为委托
        public Delegate Invoke(String APIName, Type t)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }

        public delegate bool runcmdDelegate(IntPtr intPtr);

        public delegate uint sendSimpleFormDelegate(IntPtr uuid, IntPtr title, IntPtr content, IntPtr buttons);

        public delegate uint sendModalFormDelegate(IntPtr uuid, IntPtr title, IntPtr content, IntPtr button1, IntPtr button2);

        public delegate uint sendCustomFormDelegate(IntPtr uuid, IntPtr json);

        public delegate bool destroyFormDelegate(int formId);


        /// <summary>
        /// 以后台的方式执行命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool runcmd(string cmd)
        {
            runcmdDelegate @delegate = (runcmdDelegate)Invoke("runcmd", typeof(runcmdDelegate));
            return @delegate.Invoke(cmd.GetGCHandle().AddrOfPinnedObject());
        }

        /// <summary>
        /// 发送表单
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public uint sendSimpleForm(string uuid, string title, string content, string buttons)
        {
            sendSimpleFormDelegate @delegate = (sendSimpleFormDelegate)Invoke("sendSimpleForm", typeof(sendSimpleFormDelegate));
            return @delegate.Invoke(uuid.GetGCHandle().AddrOfPinnedObject(), title.GetGCHandle().AddrOfPinnedObject(), content.GetGCHandle().AddrOfPinnedObject(), buttons.GetGCHandle().AddrOfPinnedObject());
        }

        /// <summary>
        /// 发送模式对话框
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="button1"></param>
        /// <param name="button2"></param>
        /// <returns></returns>
        public uint sendModalForm(string uuid, string title, string content, string button1, string button2)
        {
            sendModalFormDelegate @delegate = (sendModalFormDelegate)Invoke("sendModalForm", typeof(sendModalFormDelegate));
            return @delegate.Invoke(uuid.GetGCHandle().AddrOfPinnedObject(), title.GetGCHandle().AddrOfPinnedObject(), content.GetGCHandle().AddrOfPinnedObject(), button1.GetGCHandle().AddrOfPinnedObject(), button2.GetGCHandle().AddrOfPinnedObject());
        }


        /// <summary>
        /// 发送自定义窗体
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public uint sendCustomForm(string uuid, string json)
        {
            sendCustomFormDelegate @delegate = (sendCustomFormDelegate)Invoke("sendCustomForm", typeof(sendCustomFormDelegate));
            return @delegate.Invoke(uuid.GetGCHandle().AddrOfPinnedObject(), json.GetGCHandle().AddrOfPinnedObject());
        }


        /// <summary>
        /// 销毁表单ID
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public bool destroyForm(int formId)
        {
            destroyFormDelegate @delegate = (destroyFormDelegate)Invoke("destroyForm", typeof(destroyFormDelegate));
            return @delegate.Invoke(formId);
        }
    }
}
