using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Application_Interface
{

    class SplitFile
    {
        
        public string splitForBasisPairs()
        {
            List<String> liste = new List<string>();
            String[] values = File.ReadAllLines(@"E:\Dev\ProjectC#\Project-NDA\Genomes\genome-soffes.txt");
            String[] toto;
            int i = 1;
            foreach (string value in values)
            {
                toto = value.Split('\t');
                liste.Add(toto[3]);
            }

            foreach (string tab in liste)
            {
               using(StreamWriter w = File.AppendText("log.txt"))
                {
                    Log(tab.ToString(), w);
                }
            }

            //Console.ReadLine();

            return null;
        }
   

        public SplitFile()
        {

        }

        public static void Log(string logMessage, TextWriter w)
        {
            //w.Write("\r\nLog Entry : ");
            //w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            //    DateTime.Now.ToLongDateString());
            //w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            //w.WriteLine("-------------------------------");
        }
    }
}
