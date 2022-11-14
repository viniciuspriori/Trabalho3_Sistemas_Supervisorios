﻿using System;
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

namespace Trabalho3_Sistemas_Supervisorios
{
    public partial class Form1 : Form
    {
        //private OpcDaServer _server;
        public Form1()
        {

            InitializeComponent();

            StartProcedures();
        }

        public bool IsConnected => _server.IsConnected;

        public void StartProcedures()
        {
            Uri url = UrlBuilder.Build("Matrikon.OPC.Simulation.1");
            using (var server = new OpcDaServer(url))
            {
                // Connect to the server first.
                if (!server.IsConnected)
                {
                    TryConnect(server);
                }

                // Create a browser and browse all elements recursively.
                var browser = new OpcDaBrowserAuto(server);
                BrowseChildren(browser);
            }

            /// ----------GROUP----------- ///
            //// Create a group with items.
            //OpcDaGroup group = server.AddGroup("MyGroup");
            //group.IsActive = true;

            //var definition1 = new OpcDaItemDefinition
            //{
            //    ItemId = "Random.Int2",
            //    IsActive = true
            //};
            //var definition2 = new OpcDaItemDefinition
            //{
            //    ItemId = "Bucket Brigade.Int4",
            //    IsActive = true
            //};
            //OpcDaItemDefinition[] definitions = { definition1, definition2 };
            //OpcDaItemResult[] results = group.AddItems(definitions);

            //// Handle adding results.
            //foreach (OpcDaItemResult result in results)
            //{
            //    if (result.Error.Failed)
            //        Console.WriteLine("Error adding items: {0}", result.Error);
            //}

            /// ----------- READ ------------ ///

            // Read all items of the group synchronously.
            //OpcDaItemValue[] values = group.Read(group.Items, OpcDaDataSource.Device);

            // Read all items of the group asynchronously.
            //OpcDaItemValue[] values = await group.ReadAsync(group.Items);

            /// ----------- WRITE ------------ ///
            //// Prepare items.
            //OpcDaItem int2 = group.Items.FirstOrDefault(i => i.ItemId == "Bucket Brigade.Int2");
            //OpcDaItem int4 = group.Items.FirstOrDefault(i => i.ItemId == "Bucket Brigade.Int4");
            //OpcDaItem[] items = { int2, int4 };

            //// Write values to the items synchronously.
            //object[] values = { 1, 2 };
            //HRESULT[] results = group.Write(items, values);

            //// Write values to the items asynchronously.
            //object[] values = { 3, 4 };
            //HRESULT[] results = await group.WriteAsync(items, values);

            /// ----------- SUBSCRIPTION ------------ ///
            //// Configure subscription.
            //group.ValuesChanged += OnGroupValuesChanged;
            //group.UpdateRate = TimeSpan.FromMilliseconds(100); // ValuesChanged won't be triggered if zero

            //static void OnGroupValuesChanged(object sender, OpcDaItemValuesChangedEventArgs args)
            //{
            //    // Output values.
            //    foreach (OpcDaItemValue value in args.Values)
            //    {
            //        Console.WriteLine("ItemId: {0}; Value: {1}; Quality: {2}; Timestamp: {3}",
            //            value.Item.ItemId, value.Value, value.Quality, value.Timestamp);
            //    }
            //}

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

        

        void BrowseChildren(IOpcDaBrowser browser, string itemId = null, int indent = 0)
        {
            // When itemId is null, root elements will be browsed.
            OpcDaBrowseElement[] elements = browser.GetElements(itemId);

            // Output elements.
            foreach (OpcDaBrowseElement element in elements)
            {
                // Output the element.
                Console.Write(new String(' ', indent));
                Console.WriteLine(element);

                // Skip elements without children.
                if (!element.HasChildren)
                    continue;

                // Output children of the element.
                BrowseChildren(browser, element.ItemId, indent + 2);
            }
        }
    }
}
