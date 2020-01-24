using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet.core.Data
{
    [Serializable]
    public class DataSet
    {
        List<DataRow> _lines = new List<DataRow>();

        public int Count => _lines.Count;

        public int CountClasses() // Count Classes in DataSet
        {
            List<double> labels = new List<double>();
            for (int i = 0; i < _lines.Count; i++)
            {
                var label = _lines[i].GetLabel();
                if (label != -1)
                {
                    labels.Add(label);
                }
            }
            labels = labels.Distinct().ToList();
            return labels.Count;
        }

        public List<ColumnsInfo> GetColumnsInfo()
        {
            if (_lines.Count == 0) throw new Exception("DataSet is Empty");
            var rawData = _lines.Select(line => line.GetRawData()).ToList();
            var rawColumnData = new List<List<(ColumnDataType, object)>>();
            var result = new List<ColumnsInfo>();
            var firstLine = _lines[0].GetRawData();
            for (var i = 0; i < firstLine.Count; i++)
            {
                var newColumnInfo = new ColumnsInfo()
                {
                    Title = firstLine[i].Type.Title,
                    RoleType = firstLine[i].Type.Type,
                    DataType = firstLine[i].Type.DataType
                };
                result.Add(newColumnInfo);
            }
            rawData.ForEach(x => rawColumnData.Add(new List<(ColumnDataType, object)>()));
            for (var r = 0; r < rawData.Count; r++)
            {
                var row = rawData[r];
                for (var c = 0; c < row.Count; c++)
                {
                    rawColumnData[c].Add((row[c].Type.DataType, row[c].data));
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].NumberOfValues = rawColumnData[i].Count;
                if (result[i].DataType == ColumnDataType.Double && result[i].RoleType == ColumnRoleType.Data)
                {
                    result[i].Data = rawColumnData[i].Select(x => x.Item2).ToList();
                }
                else if (result[i].DataType == ColumnDataType.String && result[i].RoleType == ColumnRoleType.Label)
                {
                    result[i].Data = rawColumnData[i].Select(x => x.Item2).ToList();
                }
            }
            return result;
        }

        public int CountParameters()
        {
            return _lines[0].GetColumsQuantity();
        }

        public void Add(DataRow Row)
        {
            _lines.Add(Row);
        }

        public double[] this[int index, bool withLabel = true]
        {
            get
            {
                return _lines[index].GetLearnArray(withLabel);
            }
        }
    }
}
