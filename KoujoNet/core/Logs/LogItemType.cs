using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public enum LogItemType
    {
        Empty,
        LoadModel,
        SaveMode,
        StartLearning,
        EndLearning,
        StartPredicting,
        EndPredicting,
        LearnIterationDone
    }
}
