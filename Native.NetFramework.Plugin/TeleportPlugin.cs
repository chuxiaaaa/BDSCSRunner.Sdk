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
    public class TeleportPlugin : IPlugin
    {
        public override string pluginName => "TeleportSystem";

        public override string pluginVersion => "1.0.0";

        public override string pluginDes => "这是一个示例传送系统";

        public override string pluginAuthor => "初夏";

        public Dictionary<string, Postion> playerMove { get; set; }

        public Dictionary<string, Postion> warps { get; set; }

        public Dictionary<string, Dictionary<string, Postion>> home { get; set; }

        public Dictionary<string, Postion> deathPostion { get; set; }

        public Dictionary<string, List<string>> tpaList { get; set; }
        public Dictionary<string, List<string>> tpahereList { get; set; }


        public TeleportPlugin()
        {
            tpaList = new Dictionary<string, List<string>>();
            tpahereList = new Dictionary<string, List<string>>();
            playerMove = new Dictionary<string, Postion>();
            home = new Dictionary<string, Dictionary<string, Postion>>();
            deathPostion = new Dictionary<string, Postion>();
            warps = new Dictionary<string, Postion>();
            warps.Add("§6初夏的沙漠城", new Postion() { x = 100000, y = 100, z = 10000, dimensionId = 1 });
            this.OnPlayerInputCommandEvent += TestPlugin_OnPlayerInputCommand;
            this.OnPlayerMoveEvent += TeleportPlugin_OnPlayerMoveEvent;
            //ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
            //{
            //    while (true)
            //    {
            //        this.Info("循环输出");
            //        Thread.Sleep(1000);
            //    }
            //}));
        }

        private void TeleportPlugin_OnPlayerMoveEvent(EventArgs.PlayerMoveEventArgs e)
        {
            playerMove.Remove(e.p.playerName);
            playerMove.Add(e.p.playerName, e.p.postion);
        }

        private void TestPlugin_OnPlayerInputCommand(EventArgs.PlayerInputCommandEventArgs e)
        {
            try
            {
                if (e.Before)
                {
                    if (e.cmd.Replace(" ", "").ToLower() == "/tpagui")
                    {
                        e.Intercept();
                        var buttons = new List<Button>() {
                            new Button(){
                                Text = "warp\n传送到公共坐标",
                                clickEvent = ()=>{  WarpMethod(e); }
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
                                    TpaMethod(e);
                                }
                            },
                        };
                        if (!home.ContainsKey(e.p.playerName) || home[e.p.playerName].Count < 3)
                        {
                            buttons.Add(new Button()
                            {
                                Text = "sethome\n设置家",
                                clickEvent = () =>
                                {
                                    setHomeMethod(e);
                                }
                            });
                        }
                        if (home.ContainsKey(e.p.playerName))
                        {
                            buttons.Add(new Button()
                            {
                                Text = "home\n传送回家",
                                clickEvent = () =>
                                {
                                    HomeMethod(e);
                                }
                            });
                        }
                        if (e.p.playerName == "ChinaChuxia")
                        {
                            buttons.Add(new Button()
                            {
                                Text = "setwarp\n设置地标传送点",
                                clickEvent = () =>
                                {
                                    setWarpMethod(e);
                                }
                            });
                        }
                        e.p.SendForm(new SimpleForm()
                        {
                            Title = "快捷传送",
                            Content = "请选择你要使用的功能",
                            Buttons = buttons
                        });
                    }
                }
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx);
            }
        }

        private void TpaMethod(EventArgs.PlayerInputCommandEventArgs e)
        {
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
                    }
                }).ToList())
            });
        }

        private void setHomeMethod(EventArgs.PlayerInputCommandEventArgs e)
        {
            e.p.SendForm(new CustomForm()
            {
                Title = "快捷传送-设置" + e.p.playerName + "的家",
                Content = new List<CustomElement>() {
                    new Input(){
                        Placeholder = "家名称",
                        Text = "请输入家名称(颜色符号使用&)",
                    },
                    new Label(){
                        Text = "家所在世界:"+(e.p.postion.dimensionId == 0?"主世界":(e.p.postion.dimensionId == 1?"地狱":(e.p.postion.dimensionId == 2?"末地":"未知世界"))),
                    },
                    new Input()
                    {
                        Text = "地标X轴",
                        Value = e.p.postion.x.ToString()
                    },
                    new Input()
                    {
                        Text = "地标Y轴",
                        Value = (e.p.postion.y - e.p.Height).ToString()
                    },
                    new Input()
                    {
                        Text = "地标Z轴",
                        Value = e.p.postion.z.ToString()
                    }
                },
                submitEvent = (result) =>
                {
                    string errorMsg = "";
                    if (!float.TryParse(result[2]?.ToString(), out float x))
                    {
                        errorMsg = "X轴坐标非正确的数字";
                    }
                    if (!float.TryParse(result[3]?.ToString(), out float y))
                    {
                        errorMsg = "Y轴坐标非正确的数字";
                    }
                    if (!float.TryParse(result[4]?.ToString(), out float z))
                    {
                        errorMsg = "Z轴坐标非正确的数字";
                    }
                    if (string.IsNullOrWhiteSpace(errorMsg))
                    {
                        string homeName = result[0]?.ToString().Replace("&", "§");
                        if (!home.ContainsKey(e.p.playerName))
                        {
                            home.Add(e.p.playerName, new Dictionary<string, Postion>());
                        }
                        if (home[e.p.playerName].ContainsKey(homeName))
                        {
                            errorMsg = "家的名字不能重复!(" + homeName + "§c已经存在)";
                            e.CreateMessage(e.p).SetFormat(MessageFormat.Red).Append(errorMsg).Done();
                        }
                        home[e.p.playerName].Add(homeName, new Postion()
                        {
                            dimensionId = e.p.postion.dimensionId,
                            x = x,
                            y = y,
                            z = z
                        });
                        e.CreateMessage(e.p).SetFormat(MessageFormat.Green).Append("家").SetFormat(MessageFormat.Reset).Append(homeName).SetFormat(MessageFormat.Green).Append("设置成功!").Done();
                    }
                    else
                    {
                        e.CreateMessage(e.p).SetFormat(MessageFormat.Red).Append(errorMsg).Done();
                    }
                }
            });
        }

        private void HomeMethod(EventArgs.PlayerInputCommandEventArgs e)
        {
            var homeButtons = new List<Button>(home[e.p.playerName].Select(x => new Button()
            {
                Text = x.Key,
                clickEvent = () =>
                {
                    TeleportMethod(e, x);
                }
            }));
            string content = homeButtons.Count == 0 ? "你在该世界没有设置过家!" : "点击按钮GO BACK HOME!";
            e.p.SendForm(new SimpleForm()
            {
                Title = "快捷传送-" + e.p.playerName + "的家",
                Content = content,
                Buttons = homeButtons
            });
        }

        private void setWarpMethod(EventArgs.PlayerInputCommandEventArgs e)
        {
            e.p.SendForm(new CustomForm()
            {
                Title = "快捷传送-设置公共传送地标",
                Content = new List<CustomElement>() {
                    new Input(){
                        Placeholder = "地标名称",
                        Text = "请输入地标名称(颜色符号使用&)",
                    },
                    new Label(){
                        Text = "地标所在世界:"+(e.p.postion.dimensionId == 0?"主世界":(e.p.postion.dimensionId == 1?"地狱":(e.p.postion.dimensionId == 2?"末地":"未知世界"))),
                    },
                    new Input()
                    {
                        Text = "地标X轴",
                        Value = e.p.postion.x.ToString()
                    },
                    new Input()
                    {
                        Text = "地标Y轴",
                        Value = (e.p.postion.y - e.p.Height).ToString()
                    },
                    new Input()
                    {
                        Text = "地标Z轴",
                        Value = e.p.postion.z.ToString()
                    }
                },
                submitEvent = (result) =>
                {
                    string errorMsg = "";
                    if (!float.TryParse(result[2]?.ToString(), out float x))
                    {
                        errorMsg = "X轴坐标非正确的数字";
                    }
                    if (!float.TryParse(result[3]?.ToString(), out float y))
                    {
                        errorMsg = "Y轴坐标非正确的数字";
                    }
                    if (!float.TryParse(result[4]?.ToString(), out float z))
                    {
                        errorMsg = "Z轴坐标非正确的数字";
                    }
                    if (string.IsNullOrWhiteSpace(errorMsg))
                    {
                        string warpName = result[0]?.ToString().Replace("&", "§");
                        warps.Add(warpName, new Postion()
                        {
                            dimensionId = e.p.postion.dimensionId,
                            x = x,
                            y = y,
                            z = z
                        });
                        e.CreateMessage(e.p).SetFormat(MessageFormat.Green).Append("地标").SetFormat(MessageFormat.Reset).Append(warpName).SetFormat(MessageFormat.Green).Append("设置成功!").Done();
                    }
                    else
                    {
                        e.CreateMessage(e.p).SetFormat(MessageFormat.Red).Append(errorMsg).Done();
                    }
                }
            });
        }

        private void WarpMethod(EventArgs.PlayerInputCommandEventArgs e)
        {
            try
            {
                var warpButtons = new List<Button>(this.warps.Where(x => x.Value.dimensionId == e.p.postion.dimensionId).Select(x => new Button()
                {
                    Text = x.Key,
                    clickEvent = () =>
                    {
                        TeleportMethod(e, x);
                        //api.runcmd(string.Format("tp \"{0}\" \"{1}\"", e.p.playerName, x.Key));
                    }
                }).ToList());
                string content = warpButtons.Count == 0 ? "当前世界没有地标可以用于传送" : "公共坐标传送";
                e.p.SendForm(new SimpleForm()
                {
                    Title = "快捷传送-公共坐标传送",
                    Content = content,
                    Buttons = warpButtons
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private void TeleportMethod(EventArgs.PlayerInputCommandEventArgs e, KeyValuePair<string, Postion> x)
        {
            e.CreateMessage(e.p.playerName)
                .SetFormat(MessageFormat.Green)
                .Append("传送将在3秒后开始,请不要随意移动")
                .SetFormat(MessageFormat.Red)
                .Append("(移动将会取消传送)")
                .Done();
            int cout = 30;
            if (playerMove.ContainsKey(e.p.playerName))
                playerMove.Remove(e.p.playerName);
            playerMove.Add(e.p.playerName, e.p.postion);
            for (int i = 0; i < cout; i++)
            {
                if (!playerMove.ContainsKey(e.p.playerName))
                {
                    i--;
                    continue;
                }
                if (playerMove[e.p.playerName] == e.p.postion)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    e.CreateMessage(e.p.playerName)
                        .SetFormat(MessageFormat.Red)
                        .Append("传送已被取消!")
                        .Done();
                    break;
                }
            }
            if (playerMove.ContainsKey(e.p.playerName) && playerMove[e.p.playerName] == e.p.postion)
            {
                e.CreateMessage(e.p.playerName)
                  .SetFormat(MessageFormat.Green)
                  .Append("你已被传送到")
                  .SetFormat(MessageFormat.Reset)
                  .Append(x.Key)
                  .Done();
                api.runcmd(string.Format("tp \"{0}\" {1} {2} {3}", e.p.playerName, x.Value.x, x.Value.y, x.Value.z));
                playerMove.Remove(e.p.playerName);
            }
            else
            {
                e.CreateMessage(e.p.playerName)
                    .SetFormat(MessageFormat.Red)
                    .Append("传送已被取消!")
                    .Done();
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
