using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class Experiment : INotifyPropertyChanged
    {
        private string _experimentID;
        private string _bioRepID;
        private string _techRepID;

        

        public event PropertyChangedEventHandler PropertyChanged;

        public Experiment(string experimentID, string bioRepID, string techRepID)
        {
            _experimentID = experimentID;
            _bioRepID = bioRepID;
            _techRepID = techRepID;
        }

        public Experiment(string experimentID)
        {
            _experimentID = experimentID;
        }


        public string ExperimentID
        {
            get { return _experimentID; }
            set
            {
                _experimentID = value;
                this.NotifyPropertyChanged("ExperimentID");
            }
        }


        public string BioRepID
        {
            get { return _bioRepID; }
            set
            {
                _bioRepID = value;
                this.NotifyPropertyChanged("BioRepID");
            }
        }

        public string TechRepID
        {
            get { return _techRepID; }
            set
            {
                _techRepID = value;
                this.NotifyPropertyChanged("TechRepID");
            }
        }

        private void NotifyPropertyChanged(string name)
          {
            if(PropertyChanged != null)
              PropertyChanged(this, new PropertyChangedEventArgs(name));
          }





    }
}
