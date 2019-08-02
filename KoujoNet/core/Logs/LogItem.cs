using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoujoNet
{
    [Serializable]
    public class LogItem : ILogItem
    {
        private LogItemType _Type;
        private DateTime _Time;
        private string _Message;

        public LogItemType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        /// Time of creation
        /// </summary>
        public DateTime Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        /// <summary>
        /// Descriptive message
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

    }
}
