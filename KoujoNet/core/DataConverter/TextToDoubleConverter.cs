using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public class TextToDoubleConverter
    {
        public static double[] Convert(string str, double answer)
        {
            List<double> result = new List<double>();
            var array = str.ToArray();
            for(int i = 0; i < array.Length; i++)
            {
                result.Add((double)array[i]);
            }
            result.Add(answer);
            return result.ToArray();
        }

        public static double[] Convert(string str)
        {
            List<double> result = new List<double>();
            var array = str.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                result.Add((double)array[i]);
            }
            return result.ToArray();
        }
    }
}
