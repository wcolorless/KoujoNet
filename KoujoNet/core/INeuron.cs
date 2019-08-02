using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public interface INeuron
    {
        /// <summary>
        /// Setting a new value for the weight in neuron
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        double CorrectWeights(double delta);
        /// <summary>
        /// Compute the neuron from the input signals and return results from the activation function
        /// </summary>
        /// <param name="InputSignals"></param>
        /// <returns></returns>
        double Compute(double[] InputSignals);
        /// <summary>
        /// The Activation Function
        /// </summary>
        IActivateFunc Activate { get; }
        /// <summary>
        /// The last value of the output (activation function result)
        /// </summary>
        double LastOutput { get; set; }
        /// <summary>
        /// The last value of the sum all multiplication weights and inputs
        /// </summary>
        double LastCalcSum { get; set; }
        /// <summary>
        /// The last input signals array
        /// </summary>
        double[] LastInputSingals { get; set; }
        /// <summary>
        /// The last neuron error
        /// </summary>
        double LastError { get; set; }
        /// <summary>
        /// Provides receive the value of the weight at index
        /// </summary>
        /// <param name="index">Weight number</param>
        /// <returns></returns>
        double GetWeight(int index);
       
    }
}
