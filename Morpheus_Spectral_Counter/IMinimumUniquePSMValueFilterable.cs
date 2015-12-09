using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    interface IMinimumUniquePSMValueFilterable
    {
        double MinimumUniquePSMValueFilterable { get; set; }
        void FilterMinimumUniquePSMValue();
    }
}
