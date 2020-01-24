using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using KoujoNet.core.Data;

namespace KoujoNet.core.Algorithms.KNN
{
    public class KNNClassifier
    {
        private DataSet _dataSet;
        private List<ColumnsInfo> _columnsInfo;

        public KNNClassifier(DataSet DataSet)
        {
            _dataSet = DataSet;
 
        }
        public double GetDistance(List<double> n1, List<double> n2)
        {
            var sums = n1.Zip(n2, (D1, D2) => (D1 - D2));
            var squares = sums.Select(x => Math.Pow(x, 2));
            var sqSum = squares.Sum();
            var sqrt = Math.Sqrt(sqSum);
            return sqrt;
        }

        public void Init()
        {
            _columnsInfo = _dataSet.GetColumnsInfo();
        }

        public string Classification(DataRow inputData)
        {
            var raw = inputData.GetRawData();
            var rawData = inputData.GetLearnArray(false).ToList();
            var countItem = _columnsInfo.First().NumberOfValues;
            var needNeighbor = countItem > 3 ? (countItem / 3) + 1 : 3;
            var nd = new List<(int PositionInDataset, double Distance)>();
            for (var i = 0; i < countItem; i++)
            {
                var arrayData = _dataSet[i, false].ToList();
                var dis = GetDistance(rawData, arrayData);
                nd.Add((i, dis));
            }
            nd = nd.OrderBy(x => x.Distance).Take((int)needNeighbor).ToList();
            var labelTitles = new List<string>();
            foreach (var pos in nd)
            {
                labelTitles.Add((string)_columnsInfo.First().Data[pos.PositionInDataset]);
            }
            var g = labelTitles.GroupBy(x => x).Select(s => new {LabelName = s.Key, Value = s.Count()});
            var result = g.Where(x => x.Value == g.Max(m => m.Value)).Select(z => z.LabelName).First();
            return result;
        }
    }
}
