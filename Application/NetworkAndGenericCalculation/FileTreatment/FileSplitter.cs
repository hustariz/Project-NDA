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
        
        public static void SplitFile(string inputFile, int chunkSize)
        {

            const int BUFFER_SIZE = 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            using (Stream input = File.OpenRead(inputFile))
            {
                int index = 0;
                while (input.Position < input.Length)
                {
                    using (Stream output = File.Create("C:/Users/loika/Desktop/toto.txt" + index))
                    
                        using (MemoryStream ms = new MemoryStream())
                        {
                        {
                            int remaining = chunkSize, bytesRead;
                            while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                                    Math.Min(remaining, BUFFER_SIZE))) > 0)
                            {
                                output.Write(buffer, 0, bytesRead);
                                remaining -= bytesRead;
                            }
                        }
                        index++;
                    }

                }
                
            }
        }

        public String FileReader(string path)
        {
            string readText = File.ReadAllText(path);

            return readText;
        }


        public List<byte[]> SplitIntoChunks(string text, int chunkSize)
        {

            List<byte[]> moncul = new List<byte[]>();
            //List<string> chunks = new List<string>();
            int offset = 0;
            while (offset < text.Length)
            {
                int size = Math.Min(chunkSize, text.Length - offset);
                byte[] buffer = Encoding.ASCII.GetBytes(text.Substring(offset, size));
                moncul.Add(buffer);
                offset += size;
            }
            return moncul;
        }

        public List<string> Moncul(string str, int chunks)
        {
            var l = new List<string>();
            if (string.IsNullOrEmpty(str))
                return l;
            if (str.Length < chunks)
            {
                l.Add(str);
                return l;
            }
            int chunkSize = str.Length / chunks;

            int stringLength = str.Length;
            for (int i = 0; i < stringLength; i += chunkSize)
            {
                if (i + chunkSize > stringLength)
                    chunkSize = stringLength - i;
                l.Add(str.Substring(i, chunkSize));
            }
            string residual = "";
            l.Where((f, i) => i > chunks - 1).ToList().ForEach(f => residual += f);
            l[chunks - 1] += residual;
            return l.Take(chunks).ToList();
        }

    }
}
