using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TitaniumAS.Opc.Client;
using static TitaniumAS.Opc.Client.Interop.Common.Interop;
using TitaniumAS.Opc.Client.Interop.Common;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;
using TitaniumAS.Opc.Client.Da.Browsing;
using System.IO;
using System.Reflection.Emit;
using System.Drawing.Text;
using Timer = System.Windows.Forms.Timer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Trabalho3_Sistemas_Supervisorios
{
    public partial class Form1 : Form
    {
        private OpcDaServer server;
        private StreamWriter _debugStreamWriter;

        public const string deviceName = "Channel1.Device1";
        ConfigModel configModel;
        JsonSerializer jsonSerializer;
        string path = Path.Combine(Environment.CurrentDirectory, "configModel.json");

        public Form1()
        {
            InitializeComponent();
            AdjustControls();

            jsonSerializer = new JsonSerializer();

            configModel = new ConfigModel();
            configModel.Default();

            configModel = OpenConfigModel();

            var path = Path.Combine(Environment.CurrentDirectory, "browseelements.txt");

            _debugStreamWriter = new System.IO.StreamWriter(path);

            StartProcedures();
        }

        public void StartProcedures()
        {
            Uri url = UrlBuilder.Build($"Kepware.KEPServerEX.V6/{configModel.DeviceName}");
            server = new OpcDaServer(url);
            //using (var server = new OpcDaServer(url))
            //{

            // Connect to the server first.
            if (!server.IsConnected)
            {
                TryConnect(server);
            }

            // Create a browser and browse all elements recursively.
            if (server.IsConnected)
            {
                var browser = new OpcDaBrowserAuto(server);
                BrowseChildren(browser);
            }
            //}

            /// ----------GROUP----------- ///
            // Create a group with items.
            OpcDaGroup group = server.AddGroup("MyGroup");
            group.IsActive = true;

            var fullDictionary = new Dictionary<string, string>();
            fullDictionary = configModel.TagsRead.ToDictionary(entry => entry.Key,
                                               entry => entry.Value); //CLONE READ TAGS

            configModel.TagsWrite.ToList().ForEach(x => fullDictionary.Add(x.Key, x.Value));

            var listDefintions = new List<OpcDaItemDefinition>();

            foreach (var item in fullDictionary)
            {
                listDefintions.Add(
                    new OpcDaItemDefinition
                    {
                        ItemId = $"{deviceName}.{item.Value}",
                        IsActive = true
                    });
            }

            //var definition1 = new OpcDaItemDefinition
            //{
            //    ItemId = $"{deviceName}.Start",
            //    IsActive = true
            //};
            //var definition2 = new OpcDaItemDefinition
            //{
            //    ItemId = $"{deviceName}.Reset",
            //    IsActive = true
            //};

            //OpcDaItemDefinition[] definitions = { definition1, definition2 };
            //OpcDaItemResult[] results = group.AddItems(definitions);

            OpcDaItemResult[] results = group.AddItems(listDefintions);

            //// Handle adding results.
            foreach (OpcDaItemResult result in results)
            {
                if (result.Error.Failed)
                    MessageBox.Show($"Error adding items: {result.Error}");
            }

            /// ----------- READ ------------ ///

            //Read all items of the group synchronously.
            OpcDaItemValue[] values = group.Read(group.Items, OpcDaDataSource.Device);

            //Read all items of the group asynchronously.
            //OpcDaItemValue[] values = await group.ReadAsync(group.Items);

            // -----------WRITE------------ ///
            // Prepare items.
            OpcDaItem boolStart = group.Items.FirstOrDefault(i => i.ItemId == $"{deviceName}.{configModel.ReturnItem("BOOL_START", false)}");
            OpcDaItem boolReset = group.Items.FirstOrDefault(i => i.ItemId == $"{deviceName}.{configModel.ReturnItem("BOOL_RESET", false)}");
            OpcDaItem[] items = { boolStart, boolReset };

            // Write values to the items synchronously.
            object[] values1 = { true, true };
            HRESULT[] results1 = group.Write(items, values1);

            // Write values to the items asynchronously.
            //object[] values2 = { 3, 4 };
            //HRESULT[] results2 = await group.WriteAsync(items, values);

            var timer = new Timer()
            {
                Interval = 500
             };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            textBoxCountOpacas.Text = _textbox;
        }

        private static string _textbox;

        
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

        

        void BrowseChildren(IOpcDaBrowser browser, string itemId = null, int indent = 0)
        {
            // When itemId is null, root elements will be browsed.
            OpcDaBrowseElement[] elements = browser.GetElements(itemId);

            
            // Output elements.
            foreach (OpcDaBrowseElement element in elements)
            {
                // Output the element.
                _debugStreamWriter.Write(new String(' ', indent));
                _debugStreamWriter.WriteLine(element.Name);

                // Skip elements without children.
                if (!element.HasChildren)
                    continue;

                // Output children of the element.
                BrowseChildren(browser, element.ItemId, indent + 2);
            }
        }

        public void AdjustControls()
        {
            labelContadorDePecas.TextAlign = ContentAlignment.MiddleLeft;
            labelContadorDePecas.Location = new Point((panel1.Width / 2 - labelContadorDePecas.Width / 2), labelContadorDePecas.Location.Y);

            labelCountGeral.TextAlign = ContentAlignment.MiddleLeft;
            labelCountGeral.Location = new Point((panel1.Width / 2 - labelCountGeral.Width / 2), labelCountGeral.Location.Y);

            labelEstadoEsteira.Location = new Point((panel1.Width / 2 - labelEstadoEsteira.Width / 2), labelEstadoEsteira.Location.Y);
        }

        public ConfigModel OpenConfigModel()
        {
            if (File.Exists(path) && new FileInfo(path).Length > 0)
            {
                using (StreamReader file = File.OpenText(path))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject config = (JObject)JToken.ReadFrom(reader);
                    return JsonConvert.DeserializeObject<ConfigModel>(config.ToString());
                };
            }
            else
            {
                var newConfig = new ConfigModel();
                newConfig.Default();
                return newConfig;
            }
        }



        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            await SaveConfigModel().ConfigureAwait(true);

            Environment.Exit(0);
        }

        public async Task SaveConfigModel()
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    jsonSerializer.Serialize(writer, configModel);
                }
            }

            await Task.Delay(2000);
        }


        //DEPRECATED

        //static void OnGroupValuesChanged(object sender, OpcDaItemValuesChangedEventArgs args)
        //{
        //    // Output values.
        //    foreach (OpcDaItemValue value in args.Values)
        //    {
        //        if(value.Item.ItemId == $"{deviceName}.Emergency")
        //        {
        //            _textbox = value.Value.ToString();
        //        }
        //        //Console.WriteLine("ItemId: {0}; Value: {1}; Quality: {2}; Timestamp: {3}",
        //        //    value.Item.ItemId, value.Value, value.Quality, value.Timestamp);
        //    }
        //}


        /// ----------- SUBSCRIPTION ------------ ///
        // Configure subscription.
        //group.ValuesChanged += OnGroupValuesChanged;
        //group.UpdateRate = TimeSpan.FromMilliseconds(100); // ValuesChanged won't be triggered if zero
    }
}
