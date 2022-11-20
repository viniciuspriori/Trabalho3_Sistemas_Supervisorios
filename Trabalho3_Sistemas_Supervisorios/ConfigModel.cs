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

        public Dictionary<string, string> TagsRead { get; set; }

        public Dictionary<string, string> TagsWrite { get; set; }

        public void Default()
        {
            DeviceName = "Channel1.Device1";

            TagsRead = new Dictionary<string, string>()
            {
                ["BOOL_BUSY"] = "Busy",
                ["BOOL_EMERGENCY"] = "Emergency",
                ["BOOL_ERROR"] = "Error",
                ["WORD_NUMOPAQUE"] = "NumOpcOpaque",
                ["WORD_NUMTRANSP"] = "NumTransp", //word
                ["BOOL_OPAQUE"] = "Opaque",
                ["BOOL_TRANSP"] = "Transparent"
            };

            TagsWrite = new Dictionary<string, string>()
            {
                ["BOOL_START"] = "Start",
                ["BOOL_RESET"] = "Reset"
            };
        }

        public string ReturnItem(string key, bool isRead)
        {
            string val = "";

            if (isRead)
            {
                bool result = TagsRead.TryGetValue(key, out val);
                if (result)
                {
                    return val;
                }
                else
                {
                    return TagsRead.FirstOrDefault().Value;
                }
            }
            else
            {
                bool result = TagsWrite.TryGetValue(key, out val);
                if (result)
                {
                    return val;
                }
                else
                {
                    return TagsWrite.FirstOrDefault().Value;
                }
            }
        }



    }

}
 

