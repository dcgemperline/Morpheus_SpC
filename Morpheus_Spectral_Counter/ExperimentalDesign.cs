using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class ExperimentalDesign
    {
        public Dictionary<string, Experiment> LabeledExperimentsDictionary = new Dictionary<string, Experiment>();

        public ExperimentalDesign(List<Experiment> experimentlist)
        {
            foreach (Experiment ex in experimentlist)
            {
                LabeledExperimentsDictionary.Add(ex.ExperimentID, ex);
            }
        }
    }
}
