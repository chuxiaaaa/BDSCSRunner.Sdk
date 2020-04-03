using Native.NetFramework.Plugin.Enum;
using Native.NetFramework.Plugin.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Moudel
{
    public abstract class IPlugin
    {
        #region 属性

        public Dictionary<int, List<Delegate>> formIds { get; set; }

        /// <summary>
        /// 插件列表
        /// </summary>
        public Dictionary<string, Player> players { get; set; }
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>]Api
        /// </summary>
        public PluginApi api;
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>]AppId
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>]是否处于调试状态
        /// </summary>
        public bool debug { get; set; }
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>]输出日志的时候将信息写到文件里面
        /// </summary>
        public bool logToFile { get; set; } = true;
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>]输出日志的时候显示当前时间
        /// </summary>
        public bool logShowTime { get; set; } = true;
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>]显示的时间格式
        /// </summary>
        public string logTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>] 名称
        /// </summary>
        public abstract string pluginName { get; }
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>] 版本
        /// </summary>
        public abstract string pluginVersion { get; }
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>] 描述
        /// </summary>
        public abstract string pluginDes { get; }

        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>] 作者
        /// </summary>
        public abstract string pluginAuthor { get; }
        /// <summary>
        /// [<see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>] 日志目录
        /// </summary>
        public string logFilePath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "log\\" + pluginName + "\\";
            }
        }

        #endregion

        #region 日志相关

        private void Log(string msg, string type, ConsoleColor color)
        {
            ConsoleColor rawColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            var logInfo = string.Empty;
            if (logShowTime)
            {
                logInfo = string.Format("{0}{1}{2}", logShowTime ? "[" + DateTime.Now.ToString(logTimeFormat) + " " + type.ToUpper() + "]" : string.Empty, "[" + pluginName + "]", msg);
            }
            else
            {
                logInfo = string.Format("{0}{1}{2}{3}", logShowTime ? "[" + DateTime.Now.ToString(logTimeFormat) + "]" : string.Empty, "[" + pluginName + "]", "[" + type.ToUpper() + "]", msg);
            }
            Console.WriteLine(logInfo);
            Console.ForegroundColor = rawColor;
            if (logToFile)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(t =>
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(logFilePath);
                    if (!directoryInfo.Exists)
                        directoryInfo.Create();
                    try
                    {
                        File.AppendAllLines(logFilePath + DateTime.Now.ToString("yyyy_MM_dd") + ".log", new List<string>() { logInfo });
                    }
                    catch (Exception)
                    {

                    }
                }));
            }
        }

        /// <summary>
        /// <see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>输出日志信息
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="args"> 一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        public void Info(string msg, params object[] args)
        {
            Log(string.Format(msg, args), "info", Console.ForegroundColor);
        }

        /// <summary>
        /// <see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>输出调试信息
        /// </summary>
        /// <param name="msg">调试信息</param>
        public void Debug(string msg, params object[] args)
        {
            Log(msg, "debug", Console.ForegroundColor);
        }

        /// <summary>
        /// <see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>输出警告信息
        /// </summary>
        /// <param name="msg">警告信息</param>
        public void Warn(string msg, params object[] args)
        {
            Log(msg, "warn", ConsoleColor.Yellow);
        }

        /// <summary>
        /// <see cref="Native.NetFramework.Plugin.Moudel.IPlugin">插件</see>输出错误信息
        /// </summary>
        /// <param name="msg">错误信息</param>
        public void Error(string msg, params object[] args)
        {
            Log(msg, "error", ConsoleColor.Red);
        }
        #endregion

        #region 事件




        #region OnPlayerJoinEvent
        public delegate void OnPlayerJoinEventDelegate(PlayerJoinEventArgs e);

        /// <summary>
        /// 玩家进入游戏事件
        /// </summary>
        public event OnPlayerJoinEventDelegate OnPlayerJoinEvent;
        #endregion

        #region OnPlayerInputTextEvent

        public delegate void OnPlayerInputTextDelegate(PlayerInputTextEventArgs e);

        /// <summary>
        /// 玩家输入消息事件
        /// </summary>
        public event OnPlayerInputTextDelegate OnPlayerInputTextEvent;

        #endregion

        #region OnPlayerInputCommandEvent

        public delegate void OnPlayerInputCommandEventDelegate(PlayerInputCommandEventArgs e);

        /// <summary>
        /// 玩家输入命令事件
        /// </summary>
        public event OnPlayerInputCommandEventDelegate OnPlayerInputCommandEvent;

        #endregion

        #region OnPlayerFormSelectEvent

        public delegate void OnPlayerFormSelectEventDelegate(PlayerFormSelectEventArgs e);

        /// <summary>
        /// 玩家提交GUI事件
        /// </summary>
        public event OnPlayerFormSelectEventDelegate OnPlayerFormSelectEvent;

        #endregion

        #region OnExplodeEvent

        public delegate void OnLevelExplodeEventDelegate(LevelExploedEventArgs e);

        /// <summary>
        /// 玩家提交GUI事件
        /// </summary>
        public event OnLevelExplodeEventDelegate OnLevelExplodeEvent;

        #endregion

        #region OnPlayerMoveEvent

        public delegate void OnPlayerMoveEventDelegate(PlayerMoveEventArgs e);

        /// <summary>
        /// 玩家移动事件
        /// </summary>
        public event OnPlayerMoveEventDelegate OnPlayerMoveEvent;

        #endregion



        #region CallEvent
        public void CallEvent(EventType eventType, object eventParam)
        {
            switch (eventType)
            {
                case EventType.OnServerCmdEvent:
                    break;
                case EventType.OnServerCmdOutputEvent:
                    break;
                case EventType.OnFormSelectEvent:
                    OnPlayerFormSelectEvent?.Invoke(eventParam as PlayerFormSelectEventArgs);
                    break;
                case EventType.OnUseItemEvent:
                    break;
                case EventType.OnMoveEvent:
                    OnPlayerMoveEvent?.Invoke(eventParam as PlayerMoveEventArgs);
                    break;
                case EventType.OnAttackEvent:
                    break;
                case EventType.OnPlacedBlockEvent:
                    break;
                case EventType.OnDestroyBlockEvent:
                    break;
                case EventType.OnStartOpenChestEvent:
                    break;
                case EventType.OnStartOpenBarrelEvent:
                    break;
                case EventType.OnChangeDimensionEvent:
                    break;
                case EventType.OnPlayerJoinEvent:
                    OnPlayerJoinEvent?.Invoke(eventParam as PlayerJoinEventArgs);
                    break;
                case EventType.OnPlayerLeftEvent:
                    break;
                case EventType.OnStopOpenChestEvent:
                    break;
                case EventType.OnStopOpenBarrelEvent:
                    break;
                case EventType.OnSetSlotEvent:
                    break;
                case EventType.OnNamedMobDieEvent:
                    break;
                case EventType.OnRespawnEvent:
                    break;
                case EventType.OnChatEvent:
                    break;
                case EventType.OnInputTextEvent:
                    OnPlayerInputTextEvent?.Invoke(eventParam as PlayerInputTextEventArgs);
                    break;
                case EventType.OnInputCommandEvent:
                    OnPlayerInputCommandEvent?.Invoke(eventParam as PlayerInputCommandEventArgs);
                    break;
                case EventType.OnExploedEvent:
                    OnLevelExplodeEvent?.Invoke(eventParam as LevelExploedEventArgs);
                    break;
                case EventType.OnPlayerJumpEvent:
                    break;
                case EventType.OnRaidStartEvent:
                    break;
                case EventType.OnRaidEndEvent:
                    break;
                case EventType.OnEntityFallEvent:
                    break;
                default:
                    break;
            }

        }
        #endregion

        #endregion
    }
}
