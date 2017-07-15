using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Node
{
    //Node running on the computer where is it launched.
    class LocalNode : INode
    {


        Process p = /*get the desired process here*/;
        private PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", p.ProcessName);
        private PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName);
        //while (true)
        //{
        //    Thread.Sleep(500);
        //    double ram = ramCounter.NextValue();
        //double cpu = cpuCounter.NextValue();
        //Console.WriteLine("RAM: "+(ram/1024/1024)+" MB; CPU: "+(cpu)+" %");
        //}
        public string IpAdress => throw new NotImplementedException();

        public int ActualWorker => throw new NotImplementedException();

        public float CPuUsage => throw new NotImplementedException();

        public float MemoryUsage => throw new NotImplementedException();
    }
}
