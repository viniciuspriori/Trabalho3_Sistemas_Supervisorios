using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho3_Sistemas_Supervisorios.Config
{
    public class ConfigModel
    {
        public string DeviceName { get; set; } //nome do dispositivo
        public string ServerName { get; set; } //nome do servidor

        public int TotalizadorOpaca { get; set; } //totalizador de opacas
        public int TotalizadorTransp { get; set; } //totalizador de transparentes

        public Dictionary<int, string> Tags { get; set; } //lista de tags

        public void Default()
        {
            ServerName = "Kepware.KEPServerEX.V6";
            DeviceName = "Channel1.Device1";
            TotalizadorOpaca = 0;
            TotalizadorTransp = 0;

            Tags = new Dictionary<int, string>()
            {
                [0] = "Busy", //bool (READ)
                [1] = "Emergency", //bool (READ)
                [2] = "Error",  //bool (READ)
                [3] = "NumOpcOpaque", //word (READ)
                [4] = "NumTransp", //word (READ)
                [5] = "Opaque", //bool (READ)
                [6] = "Transparent",//bool (READ)
                [7] = "Start", //bool (WRITE)
                [8] = "Reset" //bool (WRITE)
            };
        }
    }

}
 

