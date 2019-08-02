﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public interface IEstimator
    {
        double EstimateAverage(DataSet dataset);
        double Estimate(DataSet dataset, int LineFromEstimateDataSet);
    }
}
