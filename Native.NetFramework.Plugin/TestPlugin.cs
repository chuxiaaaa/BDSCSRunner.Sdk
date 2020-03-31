using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin
{
    public class TestPlugin : IPlugin
    {
        public override string pluginName => "示例插件";

        public override string pluginVersion => "1.0.0";

        public override string pluginDes => "这是一个示例插件";

        public override string pluginAuthor => "初夏";

        public TestPlugin()
        {
            this.Info("插件信息");
            this.Warn("插件警告");
            this.Error("插件错误");
            this.OnPlayerJoinEvent += TestPlugin_OnPlayerJoinEvent;
            this.OnPlayerInputCommand += TestPlugin_OnPlayerInputCommand;
            //ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
            //{
            //    while (true)
            //    {
            //        this.Info("循环输出");
            //        Thread.Sleep(1000);
            //    }
            //}));
        }

        private void TestPlugin_OnPlayerInputCommand(EventArgs.PlayerInputCommandEventArgs e)
        {
            if (e.Before)
            {
                if (e.cmd == "/tpa")
                {
                    e.Intercept();
                    e.Warn("插件配置文件丢失");
                    e.Info("玩家输入/tpa");
                }
            }
        }

        private void TestPlugin_OnPlayerJoinEvent(EventArgs.PlayerJoinEventArgs e)
        {
            try
            {
                if (!e.Before)
                {
                    e.ReturnVoid();
                    e.Info("{0}{1}", e.p.playerName, "进入了游戏");
                    e.CreateMessage(e.p).Append("{0}{1}{2}", "§e欢迎", e.p.playerName, "进入服务器").Done();
                    Thread.Sleep(5000);
                    while (true)
                    {
                        e.Info("输出");
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }
}
