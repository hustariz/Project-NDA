using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.FileTreatment
{
    public interface IDataReader<T>
    {
        /// <summary>
        /// Returns the total data counts.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Returns the next element in the data store.
        /// This method can throw an IndexOutOfRangeException is the end of
        /// data was reached.
        /// </summary>
        T Next();

        /// <summary>
        /// Returns the N next elements in the data store.
        /// This method can throw an IndexOutOfRangeException is the end of
        /// data was reached.
        /// </summary>
        T[] Next(int length);

        /// <summary>
        /// Returns false is the reader was consummed.
        /// </summary>
        bool HasNext { get; }
    }
}
