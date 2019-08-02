using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public class SquareErrorCalc
    {
        public static double Calc(double prediction, double actual)
        {
            return Math.Pow((prediction - actual), 2) / 2;
        }
    }
}
