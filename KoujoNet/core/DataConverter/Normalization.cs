using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public class Normalization
    {
        /// <summary>
        /// Normalizes input data
        /// </summary>
        /// <param name="inputArray"></param>
        /// <returns></returns>
        public static double[] Go(double[] inputArray)
        {
            for (int i = 0; i < inputArray.Length; i++) inputArray[i] = (1.0D / inputArray[i]);
            return inputArray;
        }
    }
}
