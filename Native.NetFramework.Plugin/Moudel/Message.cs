using Native.NetFramework.Plugin.Utily;
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

        private IPlugin p { get; set; }

        /// <summary>
        /// 消息对象
        /// </summary>
        /// <param name="msgTo">消息接收人</param>
        public Message(IPlugin p, string msgTo)
        {
            this.p = p;
            this.msgTo = msgTo;
            this.msg = new StringBuilder(string.Empty);
        }

        public Message Append(string msg, params object[] args)
        {
            this.msg = this.msg.Append(string.Format(msg, args));
            return this;
        }

        /// <summary>
        /// 追加双引号
        /// </summary>
        /// <returns></returns>
        public Message AppendDqm()
        {
            this.msg = this.msg.Append("\"");
            return this;
        }

        public Message SetFormat(MessageFormat format)
        {
            string colorStr = "abcdefr";
            this.msg = this.msg.Append("§" + ((int)format < 10 ? ((int)format).ToString() : colorStr[(int)format - 10].ToString()));
            return this;
        }

        public Message AppendLine(string msg, params object[] args)
        {
            this.msg = this.msg.AppendLine(string.Format(msg, args));
            return this;
        }

        public void Done()
        {
            string cmd = "tellraw \"" + msgTo + "\" " + new { rawtext = new object[] { new { text = msg.ToString() } } }.ToJson();
            bool v = p.api.runcmd(cmd);
        }
    }

    public enum MessageFormat
    {
        /// <summary>
        /// 黑色 §0
        /// </summary>
        Black,
        /// <summary>
        /// 深蓝色 §1
        /// </summary>
        DarkBlue,
        /// <summary>
        /// 深绿色 §2
        /// </summary>
        DarkGreen,
        /// <summary>
        /// 湖蓝色 §3
        /// </summary>
        DarkAqua,
        /// <summary>
        /// 深红色 §4
        /// </summary>
        DarkRed,
        /// <summary>
        /// 紫色 §5
        /// </summary>
        DarkPurple,
        /// <summary>
        /// 金色 §6
        /// </summary>
        Gold,
        /// <summary>
        /// 灰色 §7
        /// </summary>
        Gray,
        /// <summary>
        /// 深灰色 §8
        /// </summary>
        DrakGray,
        /// <summary>
        /// 蓝色 §9
        /// </summary>
        Blue,
        /// <summary>
        /// 绿色 §a
        /// </summary>
        Green,
        /// <summary>
        /// 天蓝色 §b
        /// </summary>
        Aqua,
        /// <summary>
        /// 红色 §c
        /// </summary>
        Red,
        /// <summary>
        /// 紫色 §d
        /// </summary>
        LightPurple,
        /// <summary>
        /// 黄色 §e
        /// </summary>
        Yellow,
        /// <summary>
        /// 白色 §f
        /// </summary>
        White,
        /// <summary>
        /// 重置 §r
        /// </summary>
        Reset,


    }
}
