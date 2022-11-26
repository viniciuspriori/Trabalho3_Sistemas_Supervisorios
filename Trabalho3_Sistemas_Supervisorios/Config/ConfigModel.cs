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
            DeviceName = "OPC-Esteira.S1212DCDCDC";
            TotalizadorOpaca = 0;
            TotalizadorTransp = 0;

            Tags = new Dictionary<int, string>()
            {
                [0] = "opcOcupada", //bool (READ) BUSY
                [1] = "Emergency", //bool (READ) EMERGENCY
                [2] = "opcError",  //bool (READ)
                [3] = "opcNopaca", //word (READ)
                [4] = "opcNTransp", //word (READ)
                [5] = "opcOpaca", //bool OPACA (READ)
                [6] = "opcTransp",//bool TRANSP (READ)
                [7] = "opcStart", //bool (WRITE)
                [8] = "opcReset" //bool (WRITE)
            };

        }
    }

}
 

