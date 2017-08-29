using NetworkAndGenericCalculation.Chunk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetworkAndGenericCalculation.FileTreatment
{
    public class FileSplitter
    {
        
       
        public String FileReader(string path)
        {
            string readText = File.ReadAllText(path);

            return readText;
        }


        public Tuple<int,string> SplitIntoChunks(string text, int chunkSize, int offsets)
        {

            int offset = offsets;
            int size = Math.Min(chunkSize, text.Length - offset);
            offset += size;
            String renvoie = text.Substring(offset, size);   
            Tuple<int, string> chunkTosend = new Tuple<int, string>(offset,renvoie);
            return chunkTosend;
        }

    }
}
