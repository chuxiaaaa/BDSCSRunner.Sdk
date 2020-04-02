using Native.NetFramework.Plugin.Enum;
using Native.NetFramework.Plugin.EventArgs;
using Native.NetFramework.Plugin.Form;
using Native.NetFramework.Plugin.Utily;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Native.NetFramework.Plugin.Form.CustomForm;

namespace Native.NetFramework.Plugin.Moudel
{
    public class PluginExport
    {
        #region 导出函数

        public static IPlugin p { get; set; }

        #region OnServerStart

        [DllExport()]
        public static IntPtr OnServerStart(int AppId)
        {
            List<Type> list = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(IPlugin)).ToList();
            if (list.Count() == 0 || list.Count > 1)
            {
                Console.WriteLine("IPlugin Count >> " + list.Count);
            }
            else
            {
                Type plugin = list.FirstOrDefault();
                p = Assembly.GetExecutingAssembly().CreateInstance(plugin.FullName) as IPlugin;
                p.AppId = AppId;
                p.formIds = new Dictionary<int, List<Delegate>>();
                p.players = new Dictionary<string, Player>();
                foreach (ProcessModule item in Process.GetCurrentProcess().Modules)
                {
                    if (item.FileName.Contains("CSRunner"))
                    {
                        p.api = new PluginApi(item.FileName);
                    }
                }
                return new
                {
                    pluginName = p.pluginName,//插件名称
                    pluginVer = p.pluginVersion,//插件版本
                    pluginDes = p.pluginDes,//插件描述
                    pluginAuthor = p.pluginAuthor,//插件作者
                }.ToJson().GetGCHandle().AddrOfPinnedObject();
            }
            return IntPtr.Zero;
        }

        #endregion

        #region OnLoadName

        [DllExport]
        public static IntPtr BeforeOnLoadName(IntPtr param)
        {
            return OnPlayerJoinEvent(true, param);
        }

        [DllExport]
        public static IntPtr AfterOnLoadName(IntPtr param)
        {
            return OnPlayerJoinEvent(false, param);
        }

        private static IntPtr OnPlayerJoinEvent(bool Before, IntPtr param)
        {
            const Enum.EventType onPlayerJoinEvent = Enum.EventType.OnPlayerJoinEvent;
            string json = param.GetString();
            JObject jObject = json.ParseJsonJObject();
            EventArgs.PlayerJoinEventArgs eventParam = new EventArgs.PlayerJoinEventArgs(p);
            eventParam.Before = Before;
            eventParam.p = new Player(p)
            {
                playerName = jObject.Value<string>("playername"),
                uuid = jObject.Value<string>("uuid"),
                xuid = jObject.Value<string>("xuid"),
            };
            p.players.Remove(eventParam.p.playerName);
            p.players.Add(eventParam.p.playerName, new Player(p)
            {
                playerName = eventParam.p.playerName,
                uuid = eventParam.p.uuid,
                xuid = eventParam.p.xuid,
            });
            return CallEvent(param, onPlayerJoinEvent, eventParam);
        }

        #endregion

        #region OnInputText

        [DllExport]
        public static IntPtr BeforeOnInputText(IntPtr param)
        {
            return OnInputTextEvent(true, param);
        }

        [DllExport]
        public static IntPtr AfterOnInputText(IntPtr param)
        {
            return OnInputTextEvent(false, param);
        }

        private static IntPtr OnInputTextEvent(bool Before, IntPtr param)
        {
            const Enum.EventType e = Enum.EventType.OnInputTextEvent;
            string json = param.GetString();
            JObject jObject = json.ParseJsonJObject();
            EventArgs.PlayerInputTextEventArgs eventParam = new EventArgs.PlayerInputTextEventArgs(p);
            eventParam.Before = Before;
            var playerName = jObject.Value<string>("playername");
            if (p.players.ContainsKey(playerName))
            {
                lock (p.players[playerName])
                {
                    var pl = p.players[playerName];
                    pl.dimensionId = jObject.Value<int>("dimensionId");
                    pl.postion = new Postion()
                    {
                        x = jObject["XYZ"].Value<float>("x"),
                        y = jObject["XYZ"].Value<float>("y"),
                        z = jObject["XYZ"].Value<float>("z"),
                    };
                    pl.isstand = jObject.Value<int>("isstand");
                    p.players[playerName] = pl;
                    eventParam.p = p.players[playerName];
                    eventParam.msg = jObject.Value<string>("msg");
                }
            }
            return CallEvent(param, e, eventParam);
        }

        #endregion

        #region OnInputCommand

        [DllExport]
        public static IntPtr BeforeOnInputCommand(IntPtr param)
        {
            return OnInputCommandEvent(true, param);
        }

        [DllExport]
        public static IntPtr AfterOnInputCommand(IntPtr param)
        {
            return OnInputCommandEvent(false, param);
        }

        private static IntPtr OnInputCommandEvent(bool Before, IntPtr param)
        {
            try
            {
                const Enum.EventType OnInputCommandEvent = Enum.EventType.OnInputCommandEvent;
                string json = param.GetString();
                JObject jObject = json.ParseJsonJObject();
                EventArgs.PlayerInputCommandEventArgs eventParam = new EventArgs.PlayerInputCommandEventArgs(p);
                eventParam.Before = Before;
                var playerName = jObject.Value<string>("playername");
                if (p.players.ContainsKey(playerName))
                {
                    lock (p.players[playerName])
                    {
                        var pl = p.players[playerName];
                        pl.dimensionId = jObject.Value<int>("dimensionId");
                        pl.postion = new Postion() {
                            x = jObject["XYZ"].Value<float>("x"),
                            y = jObject["XYZ"].Value<float>("y"),
                            z = jObject["XYZ"].Value<float>("z"),
                        };
                        pl.isstand = jObject.Value<int>("isstand");
                        p.players[playerName] = pl;
                        eventParam.p = p.players[playerName];
                        eventParam.cmd = jObject.Value<string>("cmd");
                    }
                }
                return CallEvent(param, OnInputCommandEvent, eventParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IntPtr.Zero;
            }
        }
        #endregion

        #region OnFormSelect
        [DllExport]
        public static IntPtr BeforeOnFormSelect(IntPtr param)
        {
            return OnFormSelectEvent(true, param);
        }

        [DllExport]
        public static IntPtr AfterOnFormSelect(IntPtr param)
        {
            return OnFormSelectEvent(false, param);
        }

        private static IntPtr OnFormSelectEvent(bool Before, IntPtr param)
        {
            try
            {
                const Enum.EventType e = Enum.EventType.OnFormSelectEvent;
                string json = param.GetString();
                JObject jObject = json.ParseJsonJObject();
                EventArgs.PlayerFormSelectEventArgs eventParam = new EventArgs.PlayerFormSelectEventArgs(p);
                eventParam.Before = Before;
                var playerName = jObject.Value<string>("playername");
                if (p.players.ContainsKey(playerName))
                {
                    lock (p.players[playerName])
                    {
                        var pl = p.players[playerName];
                        pl.dimensionId = jObject.Value<int>("dimensionId");
                        pl.postion = new Postion()
                        {
                            x = jObject["XYZ"].Value<float>("x"),
                            y = jObject["XYZ"].Value<float>("y"),
                            z = jObject["XYZ"].Value<float>("z"),
                        };
                        pl.isstand = jObject.Value<int>("isstand");
                        p.players[playerName] = pl;
                        eventParam.p = p.players[playerName];
                        eventParam.selected = jObject.Value<string>("selected");
                        eventParam.formId = jObject.Value<int>("formid");
                    }
                    if (p.formIds.ContainsKey(eventParam.formId))
                    {
                        var d = p.formIds[eventParam.formId];
                        if (eventParam.selected == null || eventParam.selected == "null")
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
                            {
                                (d.LastOrDefault() as Action)();
                            }));
                        }
                        else if (d.Count == 2 && d[0] is FormSubmitEvent)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
                            {
                                (d[0] as FormSubmitEvent)(eventParam.selected.ParseJsonT<object[]>());
                            }));
                        }
                        else
                        {
                            if(int.TryParse(eventParam.selected,out int selected))
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
                                {
                                    (d[selected] as ButtonClickEvent)();
                                }));
                            }
                        }
                        p.formIds.Remove(eventParam.formId);
                        return new
                        {
                            intercept = true,
                        }.ToJson().GetGCHandle().AddrOfPinnedObject();
                    }
                }
                return CallEvent(param, e, eventParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IntPtr.Zero;
            }
        }
        #endregion

        #region OnExploed

        [DllExport]
        public static IntPtr BeforeOnExploed(IntPtr param)
        {
            return OnExploedEvent(true, param);
        }

        [DllExport]
        public static IntPtr AfterOnExploed(IntPtr param)
        {
            return OnExploedEvent(false, param);
        }

        private static IntPtr OnExploedEvent(bool Before, IntPtr param)
        {
            try
            {
                const Enum.EventType e = Enum.EventType.OnExploedEvent;
                string json = param.GetString();
                JObject jObject = json.ParseJsonJObject();
                EventArgs.OnExploedEventArgs eventParam = new EventArgs.OnExploedEventArgs(p);
                eventParam.Before = Before;
                eventParam.entity = jObject.Value<string>("entity");
                eventParam.entityId = jObject.Value<int>("entityId");
                eventParam.explodePower = jObject.Value<int>("explodePower");
                eventParam.dimensionId = jObject.Value<int>("dimensionId");
                eventParam.postion = new Postion()
                {
                    x = jObject["position"].Value<float>("x"),
                    y = jObject["position"].Value<float>("y"),
                    z = jObject["position"].Value<float>("z"),
                };
                return CallEvent(param, e, eventParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return IntPtr.Zero;
            }
        }
        #endregion

        private static IntPtr CallEvent(IntPtr param, EventType eventType, PluginEventArgs e)
        {
            bool intercept = false;
            bool r = false;
            EventAction action = new EventAction((bool i) =>
            {
                intercept = i;
                r = true;
            });
            e.interceptAc = action;
            ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
            {
                try
                {
                    p.CallEvent(eventType, e);
                    r = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }));
            while (!r)
            {
                Thread.Sleep(1);
                continue;
            }
            return new
            {
                intercept = intercept,
            }.ToJson().GetGCHandle().AddrOfPinnedObject();
        }

        #endregion

    }
}
