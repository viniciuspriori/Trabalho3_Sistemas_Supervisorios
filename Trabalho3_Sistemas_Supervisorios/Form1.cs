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
        ConfigManager _configManager;
        OpcManager _opcManager;
        string _loggerFolder = Path.Combine(Environment.CurrentDirectory);

        bool wStart, wReset = false;
        bool rBusy, rError, rEmergency;

        List<Control> readControls;
        Timer timerLog;

        public Form1()
        {
            InitializeComponent();

            AdjustControls();
            MoveBigLogs();

            

            readControls = new List<Control> { textBoxCountOpacas, textBoxCountTransp };

            _configManager = new ConfigManager();
            _opcManager = new OpcManager(_configManager);
            _opcManager.OnReadManager += _opcManager_OnReadManager;

            ConfigureLoggerTimer();
            Logger.AddSingleLog(0, "Application started", DateTime.Now, Logger.Status.Normal);
        }

        private void _opcManager_OnReadManager(object sender, OpcDaItemValue valuesFromManager)
        {
            CheckReceivedValues(valuesFromManager);
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

        public void ConfigureLoggerTimer()
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
                Logger.AddSingleLog(2, $"{_configManager.GetTagName(0)} Alarm went on!", DateTime.Now, Logger.Status.Normal);
            }
            if(rEmergency)
            {
                Logger.AddSingleLog(2, $"{_configManager.GetTagName(1)} Alarm went on!", DateTime.Now, Logger.Status.Emergency);
            }
            if(rError)
            {
                Logger.AddSingleLog(2, $"{_configManager.GetTagName(2)} Alarm went on!", DateTime.Now, Logger.Status.Error);
            }
        }

        private void CheckReceivedValues(OpcDaItemValue itemValue)
        {

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(0)) //BUSY - READ ALARM
            {
                rBusy = CheckAlarm(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(1)) //EMERGENCY - READ ALARM
            {
                rEmergency = CheckAlarm(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(2)) //ERROR - READ ALARM
            {
                rError = CheckAlarm(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(3)) //NUM OPACAS - READ
            {
                SetText(itemValue.Value.ToString(), readControls[0]);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(4)) //NUM TRANSP - READ
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


        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.AddSingleLog(-1, "Application is closing", DateTime.Now, Logger.Status.Normal);

            _opcManager.Kill();

            var taskModel = _configManager.SaveConfigModel();
            var taskLogger = Logger.SaveAsync(_loggerFolder);

            await Task.WhenAll(taskModel, taskLogger);

            Environment.Exit(0);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            wStart = !wStart;
            _opcManager.Write(new object[] { wStart, wReset });
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            wReset = !wReset;
            _opcManager.Write(new object[] { wStart, wReset });
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
