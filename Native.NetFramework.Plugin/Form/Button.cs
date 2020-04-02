using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.NetFramework.Plugin.Form
{
    public class Button : Element
    {
        public Image Image { get; set; }
        [JsonIgnore]
        public ButtonClickEvent clickEvent;
    }

    public delegate void ButtonClickEvent();


    public class Image
    {
        public ImageType Type { get; set; }

        [JsonProperty(propertyName: "data")]
        public string Url { get; set; }
    }

    public enum ImageType
    {
        path,
        url
    }
}
