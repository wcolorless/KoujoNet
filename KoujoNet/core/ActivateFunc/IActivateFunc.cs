using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{

    public enum ActivateType
    {
        Empty,
        Sigmoid,
        InverseSquareRoot,
        Heviside,
        HyperbolicTangent,
        HalfActivate // Своя
    }

    public interface IActivateFunc
    {
        ActivateType Type { get; set; }
        double Go(double num);
        double Dif(double num);
        double GetMax();
    }
}
