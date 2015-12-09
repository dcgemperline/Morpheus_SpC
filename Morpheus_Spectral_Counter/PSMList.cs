using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class PsmList : IQValueFilterable
    {
        public List<Psm> PeptideSpectraMatchListist = new List<Psm>();
        public double QValueThreshold { get; set; }

        public void FilterByQValue()
        {
            PeptideSpectraMatchListist = Utilities.FilterByQValue(PeptideSpectraMatchListist, QValueThreshold);
        }

        public void FilterByPredicate(Predicate<Psm> predicate)
        {
            PeptideSpectraMatchListist = PeptideSpectraMatchListist.FindAll(predicate);
        }
    }
}
