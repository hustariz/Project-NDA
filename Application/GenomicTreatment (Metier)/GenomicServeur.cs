using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkAndGenericCalculation.Sockets;
using NetworkAndGenericCalculation.MapReduce;

namespace GenomicTreatment
{
    public class GenomicServeur : Server
    {
        public GenomicServeur(IPAddress host, int portNumber, Action<string> servLogger, Action<int, string, string, float, float> gridupdater) : base(host, portNumber, servLogger, gridupdater)
        {

        }

        public override Object map(string Methodmap)
        {
            base.map(Methodmap);
            switch (Methodmap)
            {
                case "1" :

                    Console.WriteLine("J'suis MAP");

                    break;
            }
            return null;
        }
    }
}
