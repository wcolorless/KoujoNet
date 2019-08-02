using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KoujoNet
{

    [Serializable]
    public class BackPropagationLearner : ILearner
    {
        public LearnerType Type { get; set; } = LearnerType.BackPropagation;
        public ActionTypes ActionType { get; private set; }
        public double Speed { get; private set; }
        public double LearningTime { get; private set; }
        public double ResultError { get; private set; }

        private DataSet LearningData;
        private DataSet EstimateData;
        private INet Net;
        private IEstimator NetEstimator;
        private ILogger Logger;

        private BackPropagationLearner(INet Net, ILogger Logger, double Speed, ActionTypes ActionType)
        {
            this.Net = Net;
            this.Logger = Logger;
            NetEstimator = Estimator.Create(Net);
            this.Speed = Speed;
            this.ActionType = ActionType;
        }

        public static ILearner Create(INet Net, ILogger Logger, double Speed, ActionTypes ActionType)
        {
            return new BackPropagationLearner(Net, Logger, Speed, ActionType);
        }

        private double[] ComputeLayer(List<INeuron> layer, double[] InputSignals)
        {
            double[] result = new double[layer.Count];
            for (int i = 0; i < layer.Count; i++)
            {
                result[i] = layer[i].Compute(InputSignals);
            }
            return result;
        }

        private double[] Compute(List<List<INeuron>> Layers, double[] InputSignals)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                double[] result = ComputeLayer(layer, InputSignals);
                InputSignals = result;
            }
            return InputSignals;
        }

        public void Learn(List<List<INeuron>> Layers, int Iteration)
        {
            Stopwatch sw = new Stopwatch();
            List<double> errors = new List<double>();
            sw.Start();
            for (int i = 0; i < Iteration; i++)
            {
                for (int s = 0; s < LearningData.Count; s++)
                {
                    var tmpLayers = Layers.ToList();
                    var LastNeuron = tmpLayers.Last();
                    var result = Compute(tmpLayers, LearningData[s]);
                    var waitingResult = LearningData[s].Last();
                    var Error = (waitingResult - result[0]);
                    var Delta = Error * tmpLayers[0][0].Activate.Dif(LastNeuron[0].LastOutput); // Ошибка
                    for (int z = Layers.Count - 1; z >= 0; z--)
                    {
                        for (int n = 0; n < Layers[z].Count; n++)
                        {
                            if (z == (Layers.Count - 1))     Layers[z][n].CorrectWeights(Delta);
                            else
                            {
                                Delta = Layers[z + 1][0].LastError * Layers[z + 1][0].GetWeight(n) * Layers[z][n].Activate.Dif(Layers[z][n].LastOutput);
                                Layers[z][n].CorrectWeights(Delta);
                            }
                        }
                    } 
                }
                for (int ss = 0; ss < LearningData.Count; ss++)
                {
                    var tmpLayers = Layers.ToList();
                    var LastNeuron = tmpLayers.Last();
                    var result = Compute(tmpLayers, LearningData[ss]);
                    var list = new List<double>();
                    if (ActionType == ActionTypes.Classifications)
                    {

                        var tmpError = 0D;
                        for (int ln = 0; ln < LastNeuron.Count; ln++)
                        {
                            var classNum = LearningData[ss].Last();
                            if (classNum == ln)
                            {
                                tmpError = (1D - result[ln]);
                            }
                            else
                            {
                                tmpError = Math.Abs((0D - result[ln]));
                            }
                        }
                        list.Add(tmpError);
                        var avgabg = list.Average();
                        errors.Add(avgabg);
                    }
                    else
                    {
                        var waitingResult = LearningData[ss].Last();
                        var Error = Math.Abs((waitingResult - result[0]));
                        errors.Add(Error);
                    }
                }
                ResultError = errors.Average();
                errors.Clear();
                Console.WriteLine("Ошибка: " + ResultError.ToString());
            }
            sw.Stop();
            LearningTime = sw.ElapsedMilliseconds;
            Console.WriteLine("Время обучения: " + LearningTime.ToString() + " ms");
            var logItem = new LogItem() { Message = "Learning end", Time = DateTime.Now, Type = LogItemType.EndLearning };
            Logger.Add(logItem);
        }

        public void Learn(List<List<List<INeuron>>> ClassLayers, int Iteration)
        {
            for (int i = 0; i < Iteration; i++) // Iteration
            {
                for(int c = 0; c < ClassLayers.Count; c++) // Classes
                {
                    for (int s = 0; s < LearningData.Count; s++)
                    {
                        var tmpLayers = ClassLayers[c].ToList();
                        var LastNeuron = tmpLayers.Last();
                        var result = Compute(tmpLayers, LearningData[s]); // Predic
                        double waitingResult = -1;
                        if (LearningData[s].Last() != c)
                        {
                            waitingResult = 0;
                        }
                        else waitingResult = 1;
                        var Error = (waitingResult - result[0]);
                        var Delta = Error * tmpLayers[0][0].Activate.Dif(LastNeuron[0].LastOutput); // Ошибка
                        for (int z = ClassLayers[c].Count - 1; z >= 0; z--)
                        {
                            for (int n = 0; n < ClassLayers[c][z].Count; n++)
                            {
                                if (z == (ClassLayers[c].Count - 1)) ClassLayers[c][z][n].CorrectWeights(Delta);
                                else
                                {
                                    Delta = ClassLayers[c][z + 1][0].LastError * ClassLayers[c][z + 1][0].GetWeight(n) * ClassLayers[c][z][n].Activate.Dif(ClassLayers[c][z][n].LastOutput);
                                    ClassLayers[c][z][n].CorrectWeights(Delta);
                                }
                            }
                        }
                    }
                }
                // Estimate one iteration learn result
                var logItem = new LogItem() {Message = (EstimateData == null ? "Learning iteration is done" : "Learning iteration is done; Error: " + NetEstimator.EstimateAverage(EstimateData).ToString()), Time = DateTime.Now, Type = LogItemType.LearnIterationDone };
                Logger.Add(logItem);
            }
        }

        public void SetLearningData(DataSet LearningData)
        {
            this.LearningData = LearningData;
        }

        public void SetEstimateData(DataSet EstimateData)
        {
            this.EstimateData = EstimateData;
        }

    }
}
