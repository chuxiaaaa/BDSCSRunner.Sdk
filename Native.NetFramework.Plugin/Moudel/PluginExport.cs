using Native.NetFramework.Plugin.Enum;
using Native.NetFramework.Plugin.EventArgs;
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
            eventParam.p = new Player()
            {
                playerName = jObject.Value<string>("playername"),
                uuid = jObject.Value<string>("uuid"),
                xuid = jObject.Value<string>("xuid"),
            };
            p.players.Remove(eventParam.p.playerName);
            p.players.Add(eventParam.p.playerName, new Player()
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
            const Enum.EventType onPlayerJoinEvent = Enum.EventType.OnPlayerJoinEvent;
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
                    pl.postion = jObject.Value<Postion>("XYZ");
                    pl.isstand = jObject.Value<int>("isstand");
                    p.players[playerName] = pl;
                    eventParam.p = p.players[playerName];
                    eventParam.msg = jObject.Value<string>("msg");
                }
            }
            return CallEvent(param, onPlayerJoinEvent, eventParam);
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
                    pl.postion = jObject.Value<Postion>("XYZ");
                    pl.isstand = jObject.Value<int>("isstand");
                    p.players[playerName] = pl;
                    eventParam.p = p.players[playerName];
                    eventParam.cmd = jObject.Value<string>("cmd");
                }
            }
            return CallEvent(param, OnInputCommandEvent, eventParam);
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
