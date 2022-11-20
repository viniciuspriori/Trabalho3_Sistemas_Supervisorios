using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho3_Sistemas_Supervisorios
{
    public class ConfigModel
    {
        public string DeviceName { get; set; }
        public string ServerName { get; set; }

        public Dictionary<int, string> Tags { get; set; }

        public void Default()
        {
            DeviceName = "Channel1.Device1";
            ServerName = "Kepware.KEPServerEX.V6";

            Tags = new Dictionary<int, string>()
            {
                [0] = "Busy",
                [1] = "Emergency",
                [2] = "Error",
                [3] = "NumOpcOpaque",
                [4] = "NumTransp", //word
                [5] = "Opaque",
                [6] = "Transparent",
                [7] = "Start",
                [8] = "Reset"
            };
        }

        public string ReturnItem(int key, bool isRead)
        {
            string val = "";

                bool result = Tags.TryGetValue(key, out val);
                if (result)
                {
                    return val;
                }
                else
                {
                    return Tags.FirstOrDefault().Value;
                }
        }
    }

}
 

