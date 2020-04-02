using Native.NetFramework.Plugin.Form;
using Native.NetFramework.Plugin.Moudel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin
{
    public class TpaPlugin : IPlugin
    {
        public override string pluginName => "传送系统";

        public override string pluginVersion => "1.0.0";

        public override string pluginDes => "这是一个示例传送系统";

        public override string pluginAuthor => "初夏";


        public Dictionary<string, Postion> warps { get; set; }

        public Dictionary<string, Postion> deathPostion { get; set; }

        public Dictionary<string, List<string>> tpaList { get; set; }
        public Dictionary<string, List<string>> tpahereList { get; set; }


        public TpaPlugin()
        {
            tpaList = new Dictionary<string, List<string>>();
            tpahereList = new Dictionary<string, List<string>>();
            deathPostion = new Dictionary<string, Postion>();
            warps = new Dictionary<string, Postion>();
            warps.Add("§6初夏的沙漠城", new Postion() { x = 100000, y = 100, z = 10000 });
            this.OnPlayerJoinEvent += TestPlugin_OnPlayerJoinEvent;
            this.OnPlayerInputCommandEvent += TestPlugin_OnPlayerInputCommand;
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
                if (e.cmd.Replace(" ", "").ToLower() == "/tpagui")
                {
                    e.Intercept();
                    e.p.SendForm(new SimpleForm()
                    {
                        Title = "快捷传送",
                        Content = "请选择你要使用的功能",
                        Buttons = new List<Button>() {
                            new Button(){
                                Text = "warp\n传送到公共坐标",
                                clickEvent = ()=>{
                                    e.p.SendForm(new SimpleForm()
                                    {
                                        Title = "快捷传送-公共坐标传送",
                                        Content = "公共坐标传送",
                                        Buttons = new List<Button>(this.warps.Select(x => new Button()
                                        {
                                            Text = x.Key,
                                            clickEvent = () =>
                                            {
                                                e.CreateMessage(e.p.playerName)
                                                    .SetFormat(MessageFormat.Red)
                                                    .Append("传送将在3秒后开始,请不要随意移动(移动将会取消传送)")
                                                    .Done();
                                                Thread.Sleep(30*1000);
                                                api.runcmd(string.Format("tp \"{0}\" {1} {2} {3}", e.p.playerName,x.Value.x,x.Value.y,x.Value.z ));
                                                //api.runcmd(string.Format("tp \"{0}\" \"{1}\"", e.p.playerName, x.Key));
                                            }
                                        }).ToList())
                                    });
                                }
                            },
                            new Button(){
                                Text = "home\n传送到家",
                                clickEvent = ()=>{

                                }
                            },
                            new Button(){
                                Text = "back\n返回上一次死亡地点",
                                clickEvent = ()=>{

                                }
                            },

                            new Button(){
                                Text = "tpahere\n请求将对方传送到自己的位置",
                                clickEvent = ()=>{

                                }
                            },
                            new Button(){
                                Text = "tpa\n请求传送到对方的位置",
                                clickEvent = ()=>{
                                    e.p.SendForm(new SimpleForm()
                                    {
                                        Title = "快捷传送",
                                        Content = "请选择一名玩家进行TP",
                                        Buttons = new List<Button>(this.players/*.Where(x=>x.Key != e.p.playerName)*/.Select(x => new Button()
                                        {
                                            Text = x.Key,
                                            clickEvent = () =>
                                            {
                                                e.CreateMessage(e.p.playerName)
                                                    .SetFormat(MessageFormat.Green)
                                                    .Append("传送请求已发送给")
                                                    .SetFormat(MessageFormat.Yellow)
                                                    .Append(x.Key)
                                                    .SetFormat(MessageFormat.Green)
                                                    .Append(",等待对方接受!")
                                                    .Done();
                                                e.CreateMessage(x.Value)
                                                   .SetFormat(MessageFormat.Yellow)
                                                   .Append(e.p.playerName)
                                                   .SetFormat(MessageFormat.Green)
                                                   .Append("请求传送到你身边,同意传送请求请输入")
                                                   .AppendDqm()
                                                   .Append("/ty")
                                                   .AppendDqm()
                                                   .Append("(如不同意30秒后自动拒绝)!")
                                                   .Done();
                                                if (!tpaList.ContainsKey(x.Key))
                                                    tpaList.Add(x.Key, new List<string>());
                                                var RdId = Guid.NewGuid();
                                                lock (tpaList[x.Key])
                                                {
                                                    tpaList[x.Key] = tpaList[x.Key].Where(c => !c.StartsWith(e.p.playerName + "_____________")).ToList();
                                                    tpaList[x.Key].Add(e.p.playerName + "_____________" + RdId);
                                                }
                                                Thread.Sleep(30 * 1000);
                                                if (tpaList[x.Key].Contains(e.p.playerName + "_____________" + RdId))
                                                {
                                                    e.CreateMessage(e.p.playerName)
                                                        .SetFormat(MessageFormat.Yellow)
                                                        .Append(x.Key)
                                                        .SetFormat(MessageFormat.Red)
                                                        .Append("未接受你的传送请求!")
                                                        .Done();
                                                }
                                                //api.runcmd(string.Format("tp \"{0}\" \"{1}\"", e.p.playerName, x.Key));
                                            }
                                        }).ToList())
                                    });
                                }
                            },
                        }
                    });

                    //e.Info("玩家输入/tpa");
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
                    Thread.Sleep(5000);
                    e.CreateMessage(e.p).Append("{0}{1}{2}", "§e欢迎", e.p.playerName, "进入服务器").Done();
                    //while (true)
                    //{
                    //    e.Info("输出");
                    //    Thread.Sleep(1000);
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }
}
