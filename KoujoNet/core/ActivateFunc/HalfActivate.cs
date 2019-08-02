using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class HalfActivate : IActivateFunc
    {
        public static IActivateFunc Create()
        {
            return new HalfActivate();
        }

        public ActivateType Type { get; set; } = ActivateType.HalfActivate;

        public double Go(double num)
        {
            if (num <= 0.04D) return 0;
            else return 1;
        }

        public double Dif(double num)
        {
            throw new InvalidOperationException();
        }

        public double GetMax()
        {
            return 1.0D;
        }
    }
}
