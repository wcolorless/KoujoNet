using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public class SimplePresentator
    {
        /// <summary>
        /// Provides the number of active class from the model
        /// </summary>
        /// <param name="LastLayerOutputs">Results from the activation function of the model</param>
        /// <returns></returns>
        public static int Out(double[] LastLayerOutputs)
        {
            int output = -1;
            double tmp = 0;
            for(int i = 0; i < LastLayerOutputs.Length; i++)
            {
                if(LastLayerOutputs[i] > tmp)
                {
                    tmp = LastLayerOutputs[i];
                    output = i;
                }
            }
            return output;
        }
    }
}
