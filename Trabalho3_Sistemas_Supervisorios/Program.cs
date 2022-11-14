using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TitaniumAS.Opc.Client.Interop.Common.Interop;
using TitaniumAS.Opc.Client.Interop.Common;
using System.Threading;
using TitaniumAS.Opc.Client;

namespace Trabalho3_Sistemas_Supervisorios
{
    internal static class Program
    {
        //[System.Runtime.InteropServices.DllImport("ole32.dll")]
        //public static extern int CoInitializeSecurity(IntPtr pVoid, int cAuthSvc,
        //                                              IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level,
        //                                              RpcImpLevel impers, IntPtr pAuthList, EoAuthnCap dwCapabilities,
        //                                              IntPtr pReserved3);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {            //CoInitializeSecurity(IntPtr.Zero, -1, IntPtr.Zero,
            //    IntPtr.Zero, RpcAuthnLevel.None,
            //    RpcImpLevel.Impersonate, IntPtr.Zero, EoAuthnCap.None, IntPtr.Zero);

            Bootstrap.Initialize();

            var thread = new Thread(RunApplication)
            {
                Name = "RunApplicationThread",
            };
            
            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();

        }

        private static void RunApplication()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
