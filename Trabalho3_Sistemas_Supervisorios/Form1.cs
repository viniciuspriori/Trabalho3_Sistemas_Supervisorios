using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TitaniumAS.Opc.Client;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Timer = System.Windows.Forms.Timer;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;

namespace Trabalho3_Sistemas_Supervisorios
{
    public partial class Form1 : Form
    {
        private OpcDaServer server;
        //private StreamWriter _debugStreamWriter;
        private bool _isClosing;
        ConfigManager _manager;
        string _loggerFolder = Path.Combine(Environment.CurrentDirectory);

        OpcDaGroup group;
        bool wStart, wReset = false;
        bool rBusy, rError, rEmergency;

        private ReadThread readRoutine;
        private WriteThread writeRoutine;

        List<Control> readControls;
        Timer timerLog;

        public Form1()
        {
            InitializeComponent();

            MoveBigLogs();

            readControls = new List<Control> { textBoxCountOpacas, textBoxCountTransp };

            _manager = new ConfigManager();


            //var path = Path.Combine(Environment.CurrentDirectory, "browseelements.txt");
            //_debugStreamWriter = new System.IO.StreamWriter(path);

            StartOPCProcedures();

            ConfigureTimer();

            Logger.AddSingleLog(0, "Application started", DateTime.Now, Logger.Status.Normal);
        }


        public void MoveBigLogs() //Delete Larger Files
        {
            var filesInDir = Directory.GetFiles(_loggerFolder, "log*.txt");

            foreach (var file in filesInDir)
            {
                if (file.Length > 1000000)
                {
                    File.Move(file, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(file)));
                }
            }
        }

        public void StartOPCProcedures()
        {
            /// ---------- ESTABLISH CONNECTION ----------- ///
            Uri url = UrlBuilder.Build($"Kepware.KEPServerEX.V6/{configModel.DeviceName}");
            server = new OpcDaServer(url);
            {
                TryConnect(server);
            }

            /// ---------- CREATE GROUP ----------- ///
            CreateGroup();

            /// ----------- READ ------------ ///
            readRoutine = new ReadThread(group);
            readRoutine.OnReadGroup += ReadRoutine_OnReadGroup;

            /// ----------- WRITE ------------ ///
            writeRoutine = new WriteThread(group, configModel);
        }

        public void ConfigureTimer()
        {
            timerLog = new Timer();
            timerLog.Interval = 2500;
            timerLog.Tick += Timer_Log_Tick;
            timerLog.Start();
        }

        private void Timer_Log_Tick(object sender, EventArgs e)
        {
            if(rBusy)
            {
                Logger.AddSingleLog(2, $"{_manager.GetTagName(0)} Alarm went on!", DateTime.Now, Logger.Status.Normal);
            }
            if(rEmergency)
            {
                Logger.AddSingleLog(2, $"{_manager.GetTagName(1)} Alarm went on!", DateTime.Now, Logger.Status.Emergency);
            }
            if(rError)
            {
                Logger.AddSingleLog(2, $"{_manager.GetTagName(2)} Alarm went on!", DateTime.Now, Logger.Status.Error);
            }
        }

        private void ReadRoutine_OnReadGroup(object sender, List<OpcDaItemValue> valuesList)
        {
            if (!_isClosing)
                GetReadItem(valuesList);
        }

        private void GetReadItem(List<OpcDaItemValue> listfromopc)
        {
            //var busy = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[0]}");
            //var emergency = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[1]}");
            //var error = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[2]}");
            //var numOpcOpaque = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[3]}");
            //var numTransp = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[4]}");
            //var opaque = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[5]}");
            //var transp = listfromopc.FirstOrDefault(r => r.Item.ItemId == $"{configModel.DeviceName}.{configModel.TagsRead[6]}");

            for (int i = 0; i < configModel.Tags.Count; i++)
            {
                var item = listfromopc.FirstOrDefault(r => r.Item.ItemId == _manager.GetTagAddressByIndex(i));

                CheckReceivedValues(item);
            }
        }

        private void CheckReceivedValues(OpcDaItemValue itemValue)
        {

            if (itemValue.Item.ItemId == _manager.GetTagAddressByIndex(0)) //BUSY - READ ALARM
            {
                rBusy = CheckAlarm(itemValue);
            }

            if (itemValue.Item.ItemId == _manager.GetTagAddressByIndex(1)) //EMERGENCY - READ ALARM
            {
                rEmergency = CheckAlarm(itemValue);
            }

            if (itemValue.Item.ItemId == _manager.GetTagAddressByIndex(2)) //ERROR - READ ALARM
            {
                rError = CheckAlarm(itemValue);
            }

            if (itemValue.Item.ItemId == _manager.GetTagAddressByIndex(3)) //NUM OPACAS - READ
            {
                SetText(itemValue.Value.ToString(), readControls[0]);
            }

            if (itemValue.Item.ItemId == _manager.GetTagAddressByIndex(4)) //NUM TRANSP - READ
            {
                SetText(itemValue.Value.ToString(), readControls[1]);
            }
        }

        public bool CheckAlarm(OpcDaItemValue alarm)
        {
            bool val;
            var success = bool.TryParse(alarm.Value.ToString(), out val);

            if(success)
            {
                return val;
            }

            return false;
        }


        public void SetText(string text, Control control)
        {
            if (control.InvokeRequired)
            {
                Action safeWrite = delegate { SetText(text, control); };
                control.Invoke(safeWrite);
            }
            else
            {
                control.Text = text;
            }
        }

        public void CreateGroup()
        {
            // Create a group with items.
            group = server.AddGroup("MyGroup");
            group.IsActive = true;

            var listDefintions = new List<OpcDaItemDefinition>();

            foreach (var item in configModel.Tags)
            {
                listDefintions.Add(
                    new OpcDaItemDefinition
                    {
                        ItemId = $"{configModel.DeviceName}.{item.Value}",
                        IsActive = true
                    });
            }

            OpcDaItemResult[] results = group.AddItems(listDefintions);

            //// Handle adding results.
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

        public void TryConnect(OpcDaServer server)
        {
            try
            {
                server.Connect();
                Thread.Sleep(100);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.AddSingleLog(-1, "Application is closing", DateTime.Now, Logger.Status.Normal);

            _isClosing = true;
            readRoutine.CloseThread();
            writeRoutine.CloseThread();

            var taskModel = _manager.SaveConfigModel();
            var taskLogger = Logger.SaveAsync(_loggerFolder);

            await Task.WhenAll(taskModel, taskLogger);

            Environment.Exit(0);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            wStart = !wStart;
            writeRoutine.SetStart(wStart);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            wReset = !wReset;
            writeRoutine.SetReset(wReset);
        }

        public void AdjustControls()
        {
            labelContadorDePecas.TextAlign = ContentAlignment.MiddleLeft;
            labelContadorDePecas.Location = new Point((panel1.Width / 2 - labelContadorDePecas.Width / 2), labelContadorDePecas.Location.Y);

            labelCountGeral.TextAlign = ContentAlignment.MiddleLeft;
            labelCountGeral.Location = new Point((panel1.Width / 2 - labelCountGeral.Width / 2), labelCountGeral.Location.Y);

            labelEstadoEsteira.Location = new Point((panel1.Width / 2 - labelEstadoEsteira.Width / 2), labelEstadoEsteira.Location.Y);
        }
    }
}
