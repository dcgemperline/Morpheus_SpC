using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Morpheus_Spectral_Counter
{
    public class ProteomicsExperimentRunBioAndTechRepLabeled : ProteomicsExperimentRun
    {
        //This is not yet implemented and would eventually be useful when extending this tool further.

        public string BioRepLabel { get; private set; }
        public string TechRepLabel { get; private set; }

        public ProteomicsExperimentRunBioAndTechRepLabeled(ProteomicsExperimentRun per, string bioRepLabel)
        {
            BioRepLabel = bioRepLabel;
            TechRepLabel = null;
            SetupLabeledProteomicsExperimentRun(per);
        }

        public ProteomicsExperimentRunBioAndTechRepLabeled(ProteomicsExperimentRun per, string bioRepLabel, string techRepLabel)
        {
            BioRepLabel = bioRepLabel;
            TechRepLabel = techRepLabel;
            SetupLabeledProteomicsExperimentRun(per);
        }

        private void SetupLabeledProteomicsExperimentRun(ProteomicsExperimentRun per)
        {
            this.ExperimentId = per.ExperimentId.DeepClone();
            this.OutPutNSAF = per.OutPutNSAF.DeepClone();
            this.OutputDNSAF = per.OutputUNSAF.DeepClone();
            this.ProteingroupList = per.ProteingroupList.DeepClone();
            this.ProteingroupList = per.ProteingroupList.DeepClone();
            this.PsmsMatched = per.PsmsMatched.DeepClone();
            this.PsmsSummarized = per.PsmsSummarized.DeepClone();
            this.RawFileName = per.RawFileName.DeepClone();
            this.WhitelistDictionary = per.WhitelistDictionary.DeepClone();
            this.Whitelisted = per.Whitelisted.DeepClone();
        }

    }
}
