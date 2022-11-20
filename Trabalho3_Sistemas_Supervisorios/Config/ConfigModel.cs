using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho3_Sistemas_Supervisorios.Config
{
    public class ConfigModel
    {
        public string DeviceName { get; set; }
        public string ServerName { get; set; }

        public int TotalizadorOpaca { get; set; }
        public int TotalizadorTransp { get; set; }

        public Dictionary<int, string> Tags { get; set; }

        public void Default()
        {
            ServerName = "Kepware.KEPServerEX.V6";
            DeviceName = "Channel1.Device1";
            TotalizadorOpaca = 0;
            TotalizadorTransp = 0;

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


        ///deprecated///
        //public string ReturnItem(int key, bool isRead)
        //{
        //    string val = "";

        //        bool result = Tags.TryGetValue(key, out val);
        //        if (result)
        //        {
        //            return val;
        //        }
        //        else
        //        {
        //            return Tags.FirstOrDefault().Value;
        //        }
        //}
    }

}
 

