using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class HyperbolicTangentActivate : IActivateFunc
    {
        public static IActivateFunc Create()
        {
            return new HyperbolicTangentActivate();
        }

        public ActivateType Type { get; set; } = ActivateType.HyperbolicTangent;

        public double Go(double num)
        {
            var result = (Math.Exp(2D * num) - 1D) / (Math.Exp(2D * num) + 1D);
            return result;
        }

        public double Dif(double x)
        {
            //return (1.0D + Go(num)) * (1.0D - Go(num));
            return (1.0D + x) * (1.0D - x);
        }

        public double GetMax()
        {
            return 1.0D;
        }
    }
}
