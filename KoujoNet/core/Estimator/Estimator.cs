using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class Estimator : IEstimator
    {
        private INet Net;

        private Estimator(INet Net)
        {
            this.Net = Net;
        }

        public static IEstimator Create(INet Net)
        {
            return new Estimator(Net);
        }

        public double Estimate(DataSet dataset, int LineFromEstimateDataSet)
        {
           var estimatingValue =  Net.Compute(dataset[LineFromEstimateDataSet]);
           return estimatingValue[0];
        }

        public double EstimateAverage(DataSet dataset)
        {
            double resultAverageError = 0;
            double[] allErrors = new double[dataset.Count];
            for (int i = 0; i < dataset.Count; i++)
            {
                var result = Net.Compute(dataset[i]);
                var waitingResult = dataset[i].Last();
                var Error = (waitingResult - result[0]);
                allErrors[i] = Error;
            }
            resultAverageError = allErrors.Average();
            return resultAverageError;
        }
    }
}
