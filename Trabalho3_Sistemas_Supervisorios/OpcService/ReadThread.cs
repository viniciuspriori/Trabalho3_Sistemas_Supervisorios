using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Trabalho3_Sistemas_Supervisorios.OpcService
{
    public class ReadThread : IThread
    {
        public OpcDaGroup _group { get; }
        public Thread thread { get; }
        public bool IsRunning { get; set; } = true;

        public event EventHandler<List<OpcDaItemValue>> OnReadGroup; //evento de leitura

        public ReadThread(OpcDaGroup group) //thread destinada à leitura assíncrona dos itens do servidor
        {
            _group = group;
            thread = new Thread(Work);
            IsRunning = true;
            thread.Name = "ReadThread";
            thread.Start();
        }

        public void CloseThread()
        {
            IsRunning = false;
            thread.Abort();
        }

        public void Work()
        {
            while(IsRunning)
            {
                if (_group.Items.Count > 0)
                {
                    OnReadGroup?.Invoke(this, _group.Read(_group.Items, OpcDaDataSource.Device).ToList()); //assim que houver leitura, aciona evento
                }
            }
        }
    }
}
