using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public interface INet
    {
        /// <summary>
        /// Provide save method for saving model in file to path
        /// </summary>
        /// <param name="Path"></param>
        void Save(string Path);
        /// <summary>
        /// Compute the model from the input signals and return results from the activation function
        /// </summary>
        /// <param name="InputSignals"></param>
        /// <returns></returns>
        double[] Compute(double[] InputSignals);
    }
}
