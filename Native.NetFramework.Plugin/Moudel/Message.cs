using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Moudel
{
    public class Message
    {
        private StringBuilder msg { get; set; }

        private String msgTo { get; set; }

        /// <summary>
        /// 消息对象
        /// </summary>
        /// <param name="msgTo">消息接收人</param>
        public Message(IPlugin p, string msgTo)
        {
            this.msgTo = msgTo;
            this.msg = new StringBuilder(string.Empty);
        }

        public Message Append(string msg, params object[] args)
        {
            this.msg = this.msg.Append(string.Format(msg, args));
            return this;
        }

        public Message AppendLine(string msg, params object[] args)
        {
            this.msg = this.msg.AppendLine(string.Format(msg, args));
            return this;
        }

        public void Done()
        {

        }
    }
}
