using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class MorpheusSummaryFileLabeledForExperiments : MorpheusSummaryFile
    {
        public List<ProteomicsExperimentRunBioAndTechRepLabeled> Labeledperlist = new List<ProteomicsExperimentRunBioAndTechRepLabeled>();
        // need to pass this constructor back to original constructor 

        //Something Really bad is going on here, watch for circular Logic
        public MorpheusSummaryFileLabeledForExperiments(MorpheusSummaryFile msf, ExperimentalDesign exDesign) : base(Path.Combine(msf.SummaryFilePath,msf.SummaryFileIdentifier))
        {
            TransferDatafromMorpheusSummaryFile(msf);
            LabelExperiments(exDesign);
        }

        //You dont want to do this cause this will cause problems later down the road, keep everything in the Experimental Design Class and Calculate Everything there?

        private void TransferDatafromMorpheusSummaryFile(MorpheusSummaryFile msf)
        {
            this.SummaryFileIdentifier = msf.SummaryFileIdentifier;
            this.SummaryFilePath = msf.SummaryFileIdentifier;
            this.ProteomicsExperimentRunsInSummaryFile = msf.ProteomicsExperimentRunsInSummaryFile.DeepClone();
        }

        private void LabelExperiments(ExperimentalDesign exDesign)
        {
            foreach (ProteomicsExperimentRun per in ProteomicsExperimentRunsInSummaryFile)
            {
                Experiment identifiedExperiment;
                bool foundlabeledexperiment = exDesign.LabeledExperimentsDictionary.TryGetValue(per.ExperimentId, out identifiedExperiment);
                if (foundlabeledexperiment)
                {
                    ProteomicsExperimentRunBioAndTechRepLabeled
                        proteomicsExperimentRunsInSummaryFileBioandTechRepLabeledToAdd =
                            new ProteomicsExperimentRunBioAndTechRepLabeled(per, identifiedExperiment.BioRepID,
                                identifiedExperiment.TechRepID);
                    Labeledperlist.Add(proteomicsExperimentRunsInSummaryFileBioandTechRepLabeledToAdd);
                }
                else
                {
                    throw new ArgumentNullException("Experimental Design ID not found in ExperimentalDesign Object");
                }

            }
        }



    }
}
