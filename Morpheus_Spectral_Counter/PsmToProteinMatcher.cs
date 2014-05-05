using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Use linq here and contains keyword to return all instances of things that contain some substring

namespace Morpheus_Spectral_Counter
{
    public class PsmToProteinMatcher
    {
        PsmToProteinMatcher(ProteomicsExperimentRun per)
        {
            MatchPsmsToProteins(per);
        }
        public static void MatchPsmsToProteins(ProteomicsExperimentRun per)
        {
            ProteinGroupList pgl = per.ProteingroupList;
            PsmList psmlist = per.PsmList;

            foreach(Psm psm in psmlist.PeptideSpectraMatchListist)
            {
                foreach (ProteinGroup pg in pgl.Pglist)
                {
                    Boolean addedGroup = false;
                    foreach (Protein p in pg.ProteingroupList)
                    {
                        if(p.Sequence.Contains(psm.PeptideBaseSequence))
                        {
                            psm.MatchingProteinList.Add(p);
                            addedGroup = true;
                        }
                    }

                    if (psm.NumberOfProteinsMatching > 0)
                    {
                        psm.ProteinCorrectionFactor = (1.0 / psm.NumberOfProteinsMatching);
                    }

                    //This logic with addedGroup boolean adds a matching protein group only once.
                    //The logic may need to be fixed to better match abacus in choosing 1 protein group to represent the data
                    //Although, based on the TAIRdataset, everything should be unique identifiers
                    if (addedGroup)
                    {
                        psm.MatchingProteingroupList.Add(pg);
                        addedGroup = false;
                    }
                }
                if (psm.NumberOfProteingroupsMatching > 0)
                {
                    psm.ProteinGroupCorrectionFactor = (1.0 / psm.NumberOfProteingroupsMatching);
                }
                
            }
        }
    }
}
