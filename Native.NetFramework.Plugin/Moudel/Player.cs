using Native.NetFramework.Plugin.Form;
using Native.NetFramework.Plugin.Utily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Moudel
{
    public class Player
    {
        private IPlugin plugin;

        public Player(IPlugin plugin)
        {
            this.plugin = plugin;
            Height = 1.6f;

        }

        public float Height { get; private set; }

        /// <summary>
        /// 玩家名
        /// </summary>
        public string playerName { get;  set; }
        /// <summary>
        /// UUID
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// XUID
        /// </summary>
        public string xuid { get; set; }
        /// <summary>
        /// 玩家坐标
        /// </summary>
        public Postion postion { get; set; }

        /// <summary>
        /// 维度ID
        /// </summary>
        public int dimensionId { get; set; }
        /// <summary>
        /// 是否站立
        /// </summary>
        public int isstand { get; set; }

        /// <summary>
        /// 发送表单
        /// </summary>
        public void SendForm(Form.Form f)
        {
            var delegates = new List<Delegate>();
            if(f is CustomForm c)
            {
                delegates.Add(c.submitEvent);
            }
            else if(f is ModalForm m)
            {
                delegates.Add(m.button1ClickEvent);
                delegates.Add(m.button2ClickEvent);
            }
            else if (f is SimpleForm s)
            {
                delegates.AddRange(s.Buttons.Select(x=>x.clickEvent).ToList());
            }
            delegates.Add(f.FormExitEvent);
            plugin.formIds.Add(plugin.api.sendCustomForm(uuid, f.ToJson()), delegates);
        }

    }
}
