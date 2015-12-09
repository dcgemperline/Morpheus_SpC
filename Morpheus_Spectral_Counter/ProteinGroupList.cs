using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class ProteinGroupList : IQValueFilterable
    {
        public List<ProteinGroup> Pglist = new List<ProteinGroup>();
        public double QValueThreshold { get; set; }
        public double MimimumUniquePeptideValue { get; set; }
        public double MinimumUniquePSMValue { get; set; }

        public void FilterByQValue()
        {
            Pglist = Utilities.FilterByQValue(Pglist, QValueThreshold);
        }

        public void FilterbyPredicate(Predicate<ProteinGroup> predicate)
        {
            Pglist = Pglist.FindAll(predicate);
        }

    }
}
