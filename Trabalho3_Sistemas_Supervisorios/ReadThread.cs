using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Da;

namespace Trabalho3_Sistemas_Supervisorios
{
    public class ReadThread : IThread
    {
        public OpcDaGroup _group { get; }
        public Thread thread { get; }
        public bool IsRunning { get; set; } = true;

        public event EventHandler<List<OpcDaItemValue>> OnReadGroup;

        public ReadThread(OpcDaGroup group)
        {
            _group = group;
            thread = new Thread(Work);
            IsRunning = true;

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
                OnReadGroup?.Invoke(this, _group.Read(_group.Items, OpcDaDataSource.Device).ToList());
            }
        }
    }
}
