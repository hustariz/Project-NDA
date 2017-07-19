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
        
        public string splitForBasisPairs(int workerNumber)
        {
            List<String> liste = new List<string>();
            String[] values = File.ReadAllLines(@"C:\Users\loika\Source\Repos\Project-NDA3\Project-NDA\Genomes\genome-soffes.txt");

            byte[] bytes = new byte[1024];



            String[] toto;
            int i = 1;
            foreach (string value in values)
            {
                toto = value.Split('\t');

                liste.Add(toto[3]);
            }

            foreach (string tab in liste)
            {
                Console.WriteLine(tab);
            }

            Console.ReadLine();

            return null;
        }
   

        public SplitFile()
        {

        }
    }
}
