using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using KoujoNet.core.Data;

namespace KoujoNet
{

    public enum ColumnRoleType
    {
        Empty,
        Label,
        Data
    }

    public enum ColumnDataType
    {
        Empty,
        Int,
        Double,
        String
    }

    [Serializable]
    public class DataContainer
    {
        public ColumnType Type { get; private set; }
        public object data { get; private set; }

        public DataContainer(ColumnType Type, object data)
        {
            this.Type = Type;
            this.data = data;
        }

        public static DataContainer Create(ColumnType Type, object data)
        {
            return new DataContainer(Type, data);
        }
    }

    [Serializable]
    public class DataRow
    {
        List<DataContainer> Columns = new List<DataContainer>();
        public void Add(ColumnType Type, object item)
        {
            var newItem = DataContainer.Create(Type, item);
            Columns.Add(newItem);
        }

        public int GetColumsQuantity()
        {
            return Columns.Count - 1;
        }

        public double GetLabel()
        {
            var label = Columns.Find(x => x.Type.Type == ColumnRoleType.Label);
            if (label != null) return (double)label.data;
            else return -1;
        }

        public List<(ColumnType Type, object data)> GetRawData()
        {
            return Columns.Select(column => (column.Type, column.data)).ToList();
        }

        public double[] GetLearnArray(bool withLabel = true)
        {
            List<double> array = new List<double>();
            for(int i = 0; i < Columns.Count; i++)
            {
                if(Columns[i].Type.Type == ColumnRoleType.Data && Columns[i].Type.DataType == ColumnDataType.Double)
                {
                    array.Add((double)Columns[i].data);
                }
            }
            if(withLabel) array.Add((double)Columns.Find(x => x.Type.Type == ColumnRoleType.Label).data);
            return array.ToArray();
        }

    }

    [Serializable]
    public class ColumnType
    {
        public string Title { get; set; }
        public ColumnRoleType Type { get; private set; }
        public ColumnDataType DataType { get; private set; }

        private ColumnType(ColumnRoleType Type, ColumnDataType DataType, string title = null)
        {
            this.Type = Type;
            this.DataType = DataType;
            this.Title = title;
        }

        public static ColumnType Create(ColumnRoleType Type, ColumnDataType DataType, string title = null)
        {
            return new ColumnType(Type, DataType, title);
        }
    }

    [Serializable]
    public class ConsistType
    {
        public List<ColumnType> Columns = new List<ColumnType>();

        public void Add(ColumnType Type)
        {
            Columns.Add(Type);
        }
    }


    [Serializable]
    public class TextLoader // Load dataset from text file
    {
        public static DataSet LoadFromCSV(string Path, ConsistType Consist, char Separator = '\t')
        {
            DataSet newDataSet = new DataSet();
            string[] RawLines = File.ReadAllLines(Path);
            if (RawLines.Length > 0 && RawLines[0].StartsWith("#"))
            {
                var tmpArr = RawLines.ToList();
                tmpArr.RemoveAt(0);
                RawLines = tmpArr.ToArray();
            }
            for (int i = 0; i < RawLines.Length; i++)
            {
                RawLines[i] = RawLines[i].Replace('.', ','); // Можно сделать на культуре
                var elements = RawLines[i].Split(new char[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                DataRow newLine = new DataRow();
                for(int e = 0; e < elements.Length; e++)
                {
                    if(Consist.Columns[e].Type == ColumnRoleType.Label) // Class label
                    {
                        object data = null;
                        if (Consist.Columns[e].DataType == ColumnDataType.Double)
                        {
                            data = Convert.ToDouble(elements[e]);
                        }
                        else if(Consist.Columns[e].DataType == ColumnDataType.String)
                        {
                            data = elements[e];
                        }
                        else if (Consist.Columns[e].DataType == ColumnDataType.Int)
                        {
                            data = Convert.ToInt32(elements[e]);
                        }
                        newLine.Add(Consist.Columns[e], data);
                    }
                    else if(Consist.Columns[e].Type == ColumnRoleType.Data) // Cell column data 
                    {
                        object data = null;
                        if (Consist.Columns[e].DataType == ColumnDataType.Double)
                        {
                            data = Convert.ToDouble(elements[e]);
                        }
                        newLine.Add(Consist.Columns[e], data);
                    } 
                }
                newDataSet.Add(newLine);
            }
            return newDataSet;
        }
    }
}
