using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public class GenerateWeights
    {
        static Random ran = new Random(DateTime.Now.Millisecond);
        public static double Gen()
        {
            return ran.NextDouble();
        }
    }
}
