using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    interface IQValueFilterable
    {
        double QValueThreshold { get; set; }
        void FilterByQValue();
    }
}
