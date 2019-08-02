using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{

    public enum LearnerType
    {
        Empty,
        BackPropagation
    }


    public interface ILearner
    {
        LearnerType Type { get; set; }
        void SetLearningData(DataSet LearningData);
        void Learn(List<List<INeuron>> Layers, int Iteration);
        void Learn(List<List<List<INeuron>>> ClassLayers, int Iteration);
        double Speed { get; }
        double LearningTime { get; }
        double ResultError { get; }
    }
}
