using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class InverseSquareRootActivate : IActivateFunc
    {
        public static IActivateFunc Create()
        {
            return new InverseSquareRootActivate();
        }

        public ActivateType Type { get; set; } = ActivateType.InverseSquareRoot;

        public double Go(double num)
        {
            var UnderRoot = 1 + 1D * (num * num);
            var FromRoot = Math.Sqrt(UnderRoot);
            return Math.Abs(num / FromRoot);
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
