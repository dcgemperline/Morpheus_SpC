using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class ExperimentalDesign
    {
        private Dictionary<string, List<ProteomicsExperimentRunBioAndTechRepLabeled>> bioRepDictionary = new Dictionary<string, List<ProteomicsExperimentRunBioAndTechRepLabeled>>();
        
        public void AddSameBioRepExperiments(List<ProteomicsExperimentRunBioAndTechRepLabeled> perBioandTechLabeledList)
        {
            //Checks to make sure all experiments added have the same bioRepLabel
            List<string> bioRepList = new List<string>();
            foreach (ProteomicsExperimentRunBioAndTechRepLabeled perlabeled in perBioandTechLabeledList)
            {
                bioRepList.Add(perlabeled.BioRepLabel);
            }
            bool sameBioRepLabelForAllExperiments = bioRepList.All(x => x == bioRepList.First());

            //If they are all the same then add
            if (sameBioRepLabelForAllExperiments)
            {
                string bioRepLabeleKey = bioRepList.ElementAt(0);
                //Should throw an argument exception if you add duplicates
                try
                {
                    bioRepDictionary.Add(bioRepLabeleKey, perBioandTechLabeledList);
                }
                catch (Exception)
                {
                    
                    throw new Exception("Added Duplicate Keys to BioRepLabelDictionary, use unique keys for each set of bioReps");
                }
            }
        }

        public List<ProteomicsExperimentRunBioAndTechRepLabeled> ReturnExperimentsBasedOnBioRepLabel(string bioRepLabel)
        {
            return bioRepDictionary[bioRepLabel];
        }
    }
}
