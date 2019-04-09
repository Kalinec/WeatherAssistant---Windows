using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace weatherAssistant.Models
{
    class StormyMap: INotifyPropertyChanged
    {
        #region private fields

        private string _name;
        private string _url;
        

        #endregion

        #region public properties

        public string Name
        {
            get { return _name;}
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged();
            }
        }

        public List<string> Urls { get; set; }

        #endregion

        public StormyMap(string name, string url)
        {
            Name = name;
            Url = url;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
