using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trabalho3_Sistemas_Supervisorios
{
    public class ReadThread
    {
        Thread thread;
        public bool IsRunning { get; set; } = true;

        public ReadThread()
        {
            thread = new Thread(Read);

            thread.Start();
            
        }

        public void Read()
        {
            while(IsRunning)
            {

            }
        }
    }
}
