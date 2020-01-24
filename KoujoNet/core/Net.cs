using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using KoujoNet.core.Data;

namespace KoujoNet
{
    public enum NetSize
    {
        Little,
        Middle,
        Big
    }


    [Serializable]
    public class Net : INet
    {
        private ILearner Learner;
        private List<List<INeuron>> Layers = new List<List<INeuron>>();                         // Nets for Regression
        private List<List<List<INeuron>>> ClassLayerBlocks = new List<List<List<INeuron>>>();   // Nets for Classification
        private ActionTypes Action;                                                             // Selected Net Type
        private Logger ActionLogger = Logger.Create();                                          // Logging all action

        private Net(DataSet InputDataSet, ActionTypes Action, NetSize Size, ActivateType ActivateFuncType, LearnerType LearnerFuncType, double Speed, DataSet EstimeDataSet = null)
        {
            this.Action = Action;
            Learner = Learners.Create(this, ActionLogger, LearnerFuncType, Speed, Action);
            int lastLayerNeurons = 0;
            var ParametersQuantity = InputDataSet.CountParameters();
            var ClassesQuantity = InputDataSet.CountClasses();
            var layers = 3;
            if (Action == ActionTypes.Regression)
            {
                if(Size == NetSize.Little)
                {
                    layers = 2;   
                }
                else if (Size == NetSize.Middle)
                {
                    layers = 5;
                }
                else if (Size == NetSize.Big)
                {
                    layers = 7;
                }
                for (int i = 0; i < layers; i++)
                {
                    var newLayer = new List<INeuron>();
                    if (i == 0)
                    {
                        for (int n = 0; n < ParametersQuantity; n++)
                        {
                            newLayer.Add(Neuron.Create(ParametersQuantity, ActivateFuncType, Learner));
                        }
                    }
                    else if (i == (layers - 1))
                    {
                        if (Action == ActionTypes.Regression)
                        {
                            newLayer.Add(Neuron.Create(lastLayerNeurons, ActivateFuncType, Learner));
                        }
                    }
                    else
                    {
                        for (int n = 0; n < ParametersQuantity; n++)
                        {
                            newLayer.Add(Neuron.Create(lastLayerNeurons, ActivateFuncType, Learner));
                        }
                    }
                    lastLayerNeurons = newLayer.Count;
                    Layers.Add(newLayer);
                }
            }
            else if(Action == ActionTypes.Classifications)
            {
                if (Size == NetSize.Little)
                {
                    layers = 2;
                }
                else if (Size == NetSize.Middle)
                {
                    layers = 5;
                }
                else if (Size == NetSize.Big)
                {
                    layers = 7;
                }
                for (int c = 0; c < ClassesQuantity; c++) // Adding Nets for the Classes
                {
                    var newClass = new List<List<INeuron>>();
                    for (int i = 0; i < layers; i++)
                    {
                        var newLayer = new List<INeuron>();
                        if (i == 0)
                        {
                            for (int n = 0; n < ParametersQuantity; n++)
                            {
                                newLayer.Add(Neuron.Create(ParametersQuantity, ActivateFuncType, Learner));
                            }
                        }
                        else if (i == (layers - 1))
                        {
                            if (Action == ActionTypes.Classifications)
                            {
                                newLayer.Add(Neuron.Create(lastLayerNeurons, ActivateFuncType, Learner));
                            }
                        }
                        else
                        {
                            for (int n = 0; n < ParametersQuantity; n++)
                            {
                                newLayer.Add(Neuron.Create(lastLayerNeurons, ActivateFuncType, Learner));
                            }
                        }
                        lastLayerNeurons = newLayer.Count;
                        newClass.Add(newLayer);
                    }
                    ClassLayerBlocks.Add(newClass);
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// Create the net (model)
        /// </summary>
        /// <param name="InputDataSet">The dataset for training the model</param>
        /// <param name="Action">Select type of the model (Classifications or Regression)</param>
        /// <param name="Size">The size of the model</param>
        /// <param name="ActivateFuncType">Activation function for the model</param>
        /// <param name="LearnerFuncType">Learning Algorithm for the Model</param>
        /// <param name="Speed">Learning speed</param>
        /// <param name="EstimeDataSet">Dataset for evaluating a trained model</param>
        /// <returns></returns>
        public static Net Create(DataSet InputDataSet, ActionTypes Action, NetSize Size, ActivateType ActivateFuncType, LearnerType LearnerFuncType, double Speed, DataSet EstimeDataSet = null)
        {
            return new Net(InputDataSet, Action, Size, ActivateFuncType, LearnerFuncType, Speed, EstimeDataSet);
        }

        public void SetLearningData(DataSet data)
        {
            Learner.SetLearningData(data);
        }

        public void Learn(int Iteration)
        {
            if(Action == ActionTypes.Regression) Learner.Learn(Layers, Iteration);
            else if(Action == ActionTypes.Classifications) Learner.Learn(ClassLayerBlocks, Iteration);
        }

        private double[] ComputeLayer(List<INeuron> layer, double[] InputSignals)
        {
            double[] result = new double[layer.Count];
            for(int i = 0; i < layer.Count; i++)
            {
                result[i] = layer[i].Compute(InputSignals);
            }
            return result;
        }

        public double[] Compute(double[] InputSignals) // Predict
        {
            double[] ResultClass = null;
            if(Action == ActionTypes.Regression)
            {
                for (int i = 0; i < Layers.Count; i++)
                {
                    var layer = Layers[i];
                    double[] result = ComputeLayer(layer, InputSignals);
                    if (Action == ActionTypes.Classifications && i == Layers.Count - 1)
                    {
                        ResultClass = new double[result.Length];
                        ResultClass[0] = SimplePresentator.Out(result);
                        return ResultClass;
                    }
                    InputSignals = result; // Save output last layer for the next layer
                }
                return InputSignals;
            }
            else if(Action == ActionTypes.Classifications)
            {
                double[] ResultClasses = new double[ClassLayerBlocks.Count];
                var tmpInput = InputSignals.ToList();
                for (int c = 0; c < ClassLayerBlocks.Count; c++)
                {
                    InputSignals = tmpInput.ToArray();
                    var currentClass = ClassLayerBlocks[c];
                    for (int i = 0; i < currentClass.Count; i++)
                    {
                        var layer = currentClass[i];
                        double[] result = ComputeLayer(layer, InputSignals);
                        if (Action == ActionTypes.Classifications && i == currentClass.Count - 1)
                        {
                            ResultClasses[c] = result[0];
                        }
                        InputSignals = result;
                    }
                }
                ResultClasses[0] = SimplePresentator.Out(ResultClasses);
                return ResultClasses;
            }
            return InputSignals;
        }

        public void Save(string Path)
        {
            using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, this);
                fs.Close();
            }
        }

        public static Net Load(string Path)
        {
            using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                var LoadedModel = bf.Deserialize(fs) as Net;
                fs.Close();
                return LoadedModel;
            }
        }
    }
}
