using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Trabalho3_Sistemas_Supervisorios.OpcService
{
    public interface IThread //interface commum às threads
    {
        bool IsRunning { get; set; }
        OpcDaGroup _group { get; }
        Thread thread { get; }
        void Work();
        void CloseThread();


    }
}
