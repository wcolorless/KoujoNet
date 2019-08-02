using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    public interface ILogItem
    {
        LogItemType Type { get; set; }
        DateTime Time { get; set; }
        string Message { get; set; }
    }
}
