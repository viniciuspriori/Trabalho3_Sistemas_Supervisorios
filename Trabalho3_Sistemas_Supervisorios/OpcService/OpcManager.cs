using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TitaniumAS.Opc.Client;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;
using Trabalho3_Sistemas_Supervisorios.Config;

namespace Trabalho3_Sistemas_Supervisorios.OpcService
{
    public class OpcManager
    {
        ConfigManager _configManager;
        OpcDaGroup group;
        OpcDaServer server;

        private ReadThread readRoutine;
        private WriteThread writeRoutine;

        public event EventHandler<OpcDaItemValue> OnReadManager;


        public OpcManager(ConfigManager config)
        {
            _configManager = config;

            StartOPCProcedures(); //configure opc
        }

        public void Kill() //stop all threads
        {
            readRoutine.CloseThread();
            writeRoutine.CloseThread();
        }

        public void StartOPCProcedures()
        {
            /// ---------- ESTABLISH CONNECTION ----------- ///
            Uri url = UrlBuilder.Build(_configManager.GetServerUrl());
            server = new OpcDaServer(url);
            TryConnect();

            /// ---------- CREATE GROUP ----------- ///
            CreateGroup();
            /// ----------- READ ------------ ///
            readRoutine = new ReadThread(group);
            readRoutine.OnReadGroup += ReadRoutine_OnReadGroup;

            /// ----------- WRITE ------------ ///
            writeRoutine = new WriteThread(group, _configManager.GetConfigModel());
        }

        public void Write(object[] values)
        {
            writeRoutine.Write(values);
        }

        private void ReadRoutine_OnReadGroup(object sender, List<OpcDaItemValue> valuesList)
        {
            GetReadItem(valuesList);
        }

        private void GetReadItem(List<OpcDaItemValue> listfromopc)
        {
            for (int i = 0; i < _configManager.GetTagsCount(); i++)
            {
                var item = listfromopc.FirstOrDefault(r => r.Item.ItemId == _configManager.GetTagAddressByIndex(i));

                OnReadManager?.Invoke(this, item);
            }
        }


        public void TryConnect()
        {
            var count = 0;
            while (!server.IsConnected && count < 3) //try connect up to 3 times, if can't, close application
            {
                try
                {
                    count++;
                    server.Connect();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Thread.Sleep(500);
                }
            }

            if(count>3)
            {
                Logger.AddSingleLog(-1, "Application Closing", DateTime.Now, Logger.Status.Error);
                Environment.Exit(0);
            }
        }

        public void CreateGroup()
        {
            // Create a group with items.
            group = server.AddGroup("MyGroup");
            group.IsActive = true;

            var listDefintions = new List<OpcDaItemDefinition>();

            foreach (var item in _configManager.GetTags())
            {
                listDefintions.Add(
                    new OpcDaItemDefinition
                    {
                        ItemId = $"{_configManager.GetDeviceName()}.{item.Value}",
                        IsActive = true
                    });
            }

            OpcDaItemResult[] results = group.AddItems(listDefintions);

            foreach (OpcDaItemResult result in results)
            {
                if (result.Error.Failed)
                {
                    MessageBox.Show($"{result.Error}", "Application will close");
                    Logger.AddSingleLog(-100, "Application is closing", DateTime.Now, Logger.Status.Error);
                    Environment.Exit(0);
                }
            }
        }
    }
}
