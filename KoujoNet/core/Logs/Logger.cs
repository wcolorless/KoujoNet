using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace KoujoNet
{
    [Serializable]
    public class Logger : ILogger, INotifyPropertyChanged
    {
        private ObservableCollection<ILogItem> _Items = new ObservableCollection<ILogItem>();

        public ObservableCollection<ILogItem> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }

        private Logger() { }

        public static Logger Create()
        {
            return new Logger();
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Add a new item to log
        /// </summary>
        /// <param name="item"></param>
        public void Add(ILogItem item)
        {
            _Items.Add(item);
            // NotifyPropertyChanged("Items"); 
        }

        public ILogItem this[int index]
        {
            get { return _Items[index]; }
        }


    }
}
