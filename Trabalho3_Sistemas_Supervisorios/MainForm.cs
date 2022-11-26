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
using Trabalho3_Sistemas_Supervisorios.Config;
using Trabalho3_Sistemas_Supervisorios.OpcService;
using Trabalho3_Sistemas_Supervisorios.Properties;

namespace Trabalho3_Sistemas_Supervisorios
{
    public partial class MainForm : Form
    {
        ConfigManager _configManager; //gerenciador de configurações
        OpcManager _opcManager; //gerenciador de serviços OPC
        string _loggerFolder = Path.Combine(Environment.CurrentDirectory); //pasta do log

        bool wStart, wReset = false; //booleanas de escrita
        bool rBusy, rError, rEmergency, rStart, rChegouOpaca, rChegouTransp; //booleanas de leitura

        List<Control> readControls; //lista de controles associados à leitura em tempo real do OPC
        Timer timerLog, timerUI; //timer do log de evento

        int countGeralOpacas, countGeralTransp; //contadores gerais
        bool stateEmergencyLed;

        public MainForm()
        {
            InitializeComponent();

            MoveBigLogs();

            ConfigureTimers();

            readControls = new List<Control> { textBoxCountOpacas, textBoxCountTransp };

            _configManager = new ConfigManager();

            AdjustControls();

            _opcManager = new OpcManager(_configManager);
            _opcManager.OnReadManager += _opcManager_OnReadManager;


            Logger.AddSingleLog(0, "Application started", DateTime.Now, Logger.Status.Normal);
        }

        private void _opcManager_OnReadManager(object sender, OpcDaItemValue valueFromManager) //evento disparado ao receber valores lidos
        {
            if(valueFromManager != null) CheckReceivedValues(valueFromManager); 
        }

        public void MoveBigLogs() //move grandes arquivos de log para o desktop
                                  //(provavelmente há muitos erros, então deve ser visto com urgência)
        {
            var filesInDir = Directory.GetFiles(_loggerFolder, "log*.txt");

            foreach (var file in filesInDir)
            {
                if (file.Length > 1000000) //move se o tamanho do arquivo for maior do que 1MB
                {
                    File.Move(file, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(file)));
                }
            }
        }

        public void ConfigureTimers() //timer do log. escreve as tags no arquivo de texto de 2,5 em 2,5 segundos
        {
            timerLog = new Timer();
            timerLog.Interval = 2500;
            timerLog.Tick += Timer_Log_Tick;
            timerLog.Start();

            timerUI = new Timer();
            timerUI.Interval = 500;
            timerUI.Tick += TimerUI_Tick;
            timerUI.Start();
        }

        private void TimerUI_Tick(object sender, EventArgs e)
        {
            pictureBoxOnOff.Image = rStart ? Resources.lig : Resources.desl;

            if (rEmergency)
            {
                stateEmergencyLed = !stateEmergencyLed;
                TogglePictureBox();
            }
            else
            {
                stateEmergencyLed = false;
                pictureBoxEmergency.Image = Resources.lig;
            }

            ToggleLabel(rBusy, labelBusy);
            ToggleLabel(rChegouOpaca, labelChegouOpaca);
            ToggleLabel(rChegouTransp, labelChegouTransp);

        }

        private bool ToggleLabel(bool variable, Label label)
        {
            return variable ? label.Visible = true : label.Visible = false;
        }

        public void TogglePictureBox()
        {
            pictureBoxEmergency.Image = stateEmergencyLed ? Resources.lig : Resources.desl;
        }

        private void Timer_Log_Tick(object sender, EventArgs e) //adiciona ao log caso um alarme seja disparado
        {
            if(rBusy)
            {
                //escreve alarme ocupado...
                Logger.AddSingleLog(2, $"{_configManager.GetTagName(0)} Alarm went on!", DateTime.Now, Logger.Status.Normal);
            }
            if(rEmergency)
            {
                //escreve alarme emergencia...
                Logger.AddSingleLog(2, $"{_configManager.GetTagName(1)} Alarm went on!", DateTime.Now, Logger.Status.Emergency);
            }
            if(rError)
            {
                //escreve alarme erro...
                Logger.AddSingleLog(2, $"{_configManager.GetTagName(2)} Alarm went on!", DateTime.Now, Logger.Status.Error);
            }
        }

        private void CheckReceivedValues(OpcDaItemValue itemValue) //checa os valores recebidos e os distribui para a interface
        {

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(0)) //BUSY - READ ALARM
            {
                rBusy = CheckBoolean(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(1)) //EMERGENCY - READ ALARM
            {
                rEmergency = CheckBoolean(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(2)) //ERROR - READ ALARM
            {
                rError = CheckBoolean(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(3)) //NUM OPACAS - READ
            {
                SetText(itemValue.Value?.ToString(), readControls[0]);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(4)) //NUM TRANSP - READ
            {
                SetText(itemValue.Value?.ToString(), readControls[1]);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(5)) // CHEGOU OPACA - READ
            {
                rChegouOpaca = CheckBoolean(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(6)) // CHEGOU TRANSP - READ
            {
                rChegouTransp = CheckBoolean(itemValue);
            }

            if (itemValue.Item.ItemId == _configManager.GetTagAddressByIndex(7)) //START - READ/WRITE
            {
                rStart = CheckBoolean(itemValue);
            }
            
        }

        public bool CheckBoolean(OpcDaItemValue alarm) //checa estado dos alarmes
        {
            if (alarm.Value != null)
            {
                bool val;
                var success = bool.TryParse(alarm.Value.ToString(), out val);

                if (success)
                {
                    return val;
                }

                return false;
            }
            return false;
        }

        public void SetText(string text, Control control) //atualiza o texto de acordo com o número de peças opacas e transparentes
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

        private void buttonStart_Click(object sender, EventArgs e) //atualiza o estado de start ao botão ser pressionado e escreve a tag
        {
            wStart = !wStart;
            _opcManager.Write(new object[] { wStart, wReset });
        }

        private void buttonReset_Click(object sender, EventArgs e) //atualiza o estado de reset ao botão ser pressionado e escreve a tag
        {
            wReset = !wReset;

            if (wReset == true)
            {
                GetValuesFromTextBoxes();
            }

            _opcManager.Write(new object[] { wStart, wReset });

        }

        public void GetValuesFromTextBoxes() //contagem de totalizadores
        {
            countGeralOpacas += int.Parse(textBoxCountOpacas.Text);
            textBoxCountGeralOpacas.Text = countGeralOpacas.ToString();

            countGeralTransp += int.Parse(textBoxCountTransp.Text);
            textBoxCountGeralTransp.Text = countGeralTransp.ToString();

            _configManager.UpdateTotalizers(countGeralOpacas, countGeralTransp); //alimenta os totalizadores no modelo
        }

        public void AdjustControls() //centraliza os controles na interface
        {

            var tupleTotalizers = _configManager.GetTotalizers();
            countGeralOpacas = tupleTotalizers.Item1;
            countGeralTransp = tupleTotalizers.Item2;
            textBoxCountGeralOpacas.Text = tupleTotalizers.Item1.ToString();
            textBoxCountGeralTransp.Text = tupleTotalizers.Item2.ToString();
            labelBusy.Visible = false;

            //

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            
            pictureBoxEsteira.Image = Resources.esteira;
            pictureBoxEmergency.Image = Resources.lig;
            pictureBoxOnOff.Image = Resources.desl;

            labelContadorDePecas.TextAlign = ContentAlignment.MiddleLeft;
            labelContadorDePecas.Location = new Point((panel1.Width / 2 - labelContadorDePecas.Width / 2), labelContadorDePecas.Location.Y);

            labelCountGeral.TextAlign = ContentAlignment.MiddleLeft;
            labelCountGeral.Location = new Point((panel1.Width / 2 - labelCountGeral.Width / 2), labelCountGeral.Location.Y);

            labelBusy.TextAlign = ContentAlignment.MiddleLeft;
            labelBusy.Location = new Point((panel1.Width / 2 - labelBusy.Width / 2), labelBusy.Location.Y);
        }
    }
}
