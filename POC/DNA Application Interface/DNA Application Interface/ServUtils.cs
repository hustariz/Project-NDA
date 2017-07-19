using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Application_Interface
{
    class ServUtils
    {


        public ServUtils()
        {

        }

        public string divideGenomeByWorker(int worker)
        {

            List<String> liste = new List<string>();
            var lineCount = File.ReadAllLines(@"E:\Dev\ProjectC#\Project-NDA\Genomes\genome-soffes.txt").Count();
            String[] values = File.ReadAllLines(@"E:\Dev\ProjectC#\Project-NDA\Genomes\genome-soffes.txt");

            int maximumSize = lineCount % worker;

            //int count = File.ReadLines(path).Count();





            if (maximumSize == 0)
            {
                for (int i = 0; i < lineCount / worker; i++)
                {
                    List <String> newValues = new List<string>();
                    newValues.Add(values[i]);
                }
            }


            return values[9];

        }


    }
}
