using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace KoujoNet.core.Data
{
    public class ColumnsInfo
    {
        public string Title { get; set; }
        public ColumnRoleType RoleType { get; set; }
        public ColumnDataType DataType { get; set; }
        public long NumberOfValues { get; set; }
        public List<object> Data { get; set; }
    }
}
