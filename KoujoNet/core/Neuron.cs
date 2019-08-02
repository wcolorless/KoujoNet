using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class Neuron : INeuron
    {
        private double[] Weights;
        private double bias;

        public double GetWeight(int index)
        {
             return Weights[index];
        }
        public IActivateFunc Activate { get; private set; }
        ILearner Learner;
        public double LastOutput { get; set; } = 0;
        public double LastCalcSum { get; set; } = 0;
        public double[] LastInputSingals { get; set; } = null;
        public double LastError { get; set; } = 0;
        private Neuron(int InputSize, ActivateType ActivateFuncType, ILearner Learner)
        {
            this.Learner = Learner;
            Activate = Activates.Create(ActivateFuncType);
            Weights = new double[InputSize];
            InitStartValues(Weights);
        }

        public static Neuron Create(object InputSize, ActivateType ActivateFuncType, ILearner Learner)
        {
            int input = 0;
            if (InputSize.GetType() == typeof(int)) input = (int)InputSize;
            else if (InputSize.GetType() == typeof(string)) input = ((string)InputSize).Length;
            return new Neuron(input, ActivateFuncType, Learner);
        }

        private void InitStartValues(double[] array)
        {
            Random ran = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = ran.NextDouble();
            }
            bias = ran.NextDouble();
        }

        public double CorrectWeights(double delta)
        {
            Random ran = new Random();
            LastError = delta;
            for (int i = 0; i < Weights.Length; i++)
            {
                var newWeight = delta * LastInputSingals[i];
                Weights[i] += newWeight;
            }
            bias += delta;
            return 0;
        }

        public double Compute(double[] InputSignals)
        {
            double result = 0;
            LastInputSingals = InputSignals;
            var preResult = InputSignals.Zip(Weights, (x, y) => x * y).ToArray();
            for (int i = 0; i < Weights.Length; i++)
            {
                result += preResult[i];
            }
            result += bias;
            LastCalcSum = result;
            LastOutput = Activate.Go(result);
            return LastOutput;
        }

    }
}
