using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;

namespace Trabalho3_Sistemas_Supervisorios
{
    public class WriteThread : IThread
    {
        public bool IsRunning { get; set; }

        public OpcDaGroup _group { get; }

        public Thread thread { get; }

        public bool wStart { get; set; }
        public bool wReset { get; set; }
        public ConfigModel configModel { get; }

        object _synclock;

        public WriteThread(OpcDaGroup group, ConfigModel model)
        {
            _group = group;
            configModel = model;

            thread = new Thread(Work);
            IsRunning = true;
            _synclock = new object();
            thread.Start();
        }

        public void SetStart(bool val)
        {
            wStart = val;
        }

        public void SetReset(bool val)
        {
            wReset = val;
        }

        public void CloseThread()
        {
            IsRunning = false;
            thread.Abort();
        }

        public void Work()
        {
            while (IsRunning)
            {
                lock (_synclock)
                {
                    var writeItems = GetWriteItems();

                    object[] values = { wStart, wReset };

                    HRESULT[] results = _group.Write(writeItems, values);
                }
            }
        }

        private List<OpcDaItem> GetWriteItems()
        {
            var list = new List<OpcDaItem>()
            {
                _group.Items.FirstOrDefault(i => i.ItemId == $"{configModel.DeviceName}.{configModel.Tags[7]}"),
                _group.Items.FirstOrDefault(i => i.ItemId == $"{configModel.DeviceName}.{configModel.Tags[8]}")
            };

            return list.Where(i => i != null).ToList();
        }
    }
}
