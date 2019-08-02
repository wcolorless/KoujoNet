using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public class Learners
    {
        /// <summary>
        /// Returns a learning algorithm object
        /// </summary>
        /// <param name="Net"></param>
        /// <param name="Logger"></param>
        /// <param name="Type"></param>
        /// <param name="Speed"></param>
        /// <param name="ActionType"></param>
        /// <returns></returns>
        public static ILearner Create(INet Net, ILogger Logger, LearnerType Type, double Speed, ActionTypes ActionType)
        {
            if (Type == LearnerType.BackPropagation)
            {
                return BackPropagationLearner.Create(Net, Logger, Speed, ActionType);
            }
            else return null;
        }
    }
}
