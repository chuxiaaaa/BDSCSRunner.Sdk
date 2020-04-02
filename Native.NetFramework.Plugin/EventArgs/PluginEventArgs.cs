using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.EventArgs
{
    public class PluginEventArgs
    {
        private IPlugin p { get; set; }

        public EventAction interceptAc { get; set; }

        /// <summary>
        /// 如果<see cref="PluginEventArgs.Before">Before</see>为<see cref="bool.TrueString">True</see>表明该事件可拦截
        /// </summary>
        public bool Before { get; set; }

        public PluginEventArgs(IPlugin p)
        {
            this.p = p;
        }

        /// <summary>
        /// 放行事件
        /// </summary>
        public void ReturnVoid()
        {
            interceptAc(false);
        }

        /// <summary>
        /// 拦截该事件
        /// </summary>
        public void Intercept()
        {
            interceptAc(true);
        }

        /// <summary>   
        /// 该方法指向<seealso cref="IPlugin.Info(string, object[])">IPlugin.Info</seealso>
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args"> 一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        public void Info(string msg, params object[] args) => p.Info(msg, args);

        /// <summary>   
        /// 该方法指向<seealso cref="IPlugin.Warn(string, object[])">IPlugin.Warn</seealso>
        /// </summary>
        /// <param name="msg">警告信息</param>
        /// <param name="args"> 一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        public void Warn(string msg, params object[] args) => p.Warn(msg, args);

        /// <summary>   
        /// 该方法指向<seealso cref="IPlugin.Error(string, object[])">IPlugin.Error</seealso>
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="args"> 一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        public void Error(string msg, params object[] args) => p.Error(msg, args);


        /// <summary>
        /// 创建消息对象
        /// </summary>
        /// <param name="msgTo"></param>
        public Message CreateMessage(string msgTo)
        {
            return new Message(this.p, msgTo);
        }

        /// <summary>
        /// 创建消息对象
        /// </summary>
        /// <param name="msgTo"></param>
        public Message CreateMessage(Player p)
        {
            return new Message(this.p, p.playerName);
        }

    }
}
