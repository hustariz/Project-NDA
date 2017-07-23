using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.FileTreatment
{
    class FileSplitter
    {
        public static void SplitFile(string inputFile, int chunkSize)
        {
            byte[] fileArray = new byte[chunkSize];
            using (Stream input = File.OpenRead(inputFile))
            {
                int bytesAct = 1;
            }

                try
            {
                int bytesAct = StreamReader(fileArray, 0, chunkSize);
                if (bytesAct != chunkSize)
                { //to make sure there is no empty spaces
                    byte[] toReturn = new byte[bytesAct];
                    for (int i = 0; i < toReturn.length; i++)
                    {
                        toReturn[i] = fileArray[i];
                    }
                    return toReturn;
                }
            }
            catch (FileNotFoundException e)
            {
                e.printStackTrace();
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
            return fileArray;
        }
    }
}
