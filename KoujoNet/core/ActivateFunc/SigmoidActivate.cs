using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class SigmoidActivate : IActivateFunc
    {
        public static IActivateFunc Create()
        {
            return new SigmoidActivate();
        }

        public ActivateType Type { get; set; } = ActivateType.Sigmoid;

        public double Go(double x)
        {
            var result = 1D / (1D + Math.Exp(-x));
            return result;
        }

        public double Dif(double x)
        {
            //return (1.0D - Go(num)) * Go(num);
            return x * (1D - x);
        }

        public double GetMax()
        {
            return 1.0D;
        }
    }
}
