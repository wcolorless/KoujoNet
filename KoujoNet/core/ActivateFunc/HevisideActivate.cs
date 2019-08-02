using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class HevisideActivate : IActivateFunc
    {
        public static IActivateFunc Create()
        {
            return new HevisideActivate();
        }

        public ActivateType Type { get; set; } = ActivateType.Heviside;

        public double Go(double num)
        {
            if (num <= 0) return 0;
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
