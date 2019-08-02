using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{

    public class Activates
    {
        /// <summary>
        /// Provides all types of activation functions
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static IActivateFunc Create(ActivateType Type)
        {
            if (Type == ActivateType.Sigmoid)
            {
                return SigmoidActivate.Create();
            }
            else if(Type == ActivateType.InverseSquareRoot)
            {
                return InverseSquareRootActivate.Create();
            }
            else if(Type == ActivateType.Heviside)
            {
                return HevisideActivate.Create();
            }
            else if(Type == ActivateType.HyperbolicTangent)
            {
                return HyperbolicTangentActivate.Create();
            }
            else if(Type == ActivateType.HalfActivate)
            {
                return HalfActivate.Create();
            }
            else return null;
        }
    }
}
