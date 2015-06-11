using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public static class PsmSummarizer
    {
        public static void CalculateUniqueAndSharedPsms(ProteomicsExperimentRun per)
        {
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                pg.UniquePsms = 0;
                //pg.SharedPsms = 0;
                int sharedpsms = 0;
                //double correctedpsms = 0;
                foreach (Psm psm in per.PsmList.PeptideSpectraMatchListist)
                {
                    
                    //  This should only contain unique peptides
                    if ((psm.MatchingProteingroupList.Count == 1) && (psm.MatchingProteingroupList.Contains(pg)))
                    {
                        pg.UniquePsms++;
                    }
                    if (psm.MatchingProteingroupList.Contains(pg))
                    {
                        sharedpsms++;
                    }
                    if (psm.MatchingProteingroupList.Contains(pg) && (psm.MatchingProteingroupList.Count > 1))
                    {
                        foreach (ProteinGroup sharedpg in psm.MatchingProteingroupList)
                        {
                            if (!pg.SharedGroupList.Contains(sharedpg))
                            {
                                pg.SharedGroupList.Add(sharedpg);
                            }
                        }
                    }
                }
                pg.SharedPsms = sharedpsms - pg.UniquePsms;
            }
            
        }

        public static void CalculateAdjustedPsms(ProteomicsExperimentRun per)
        {
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                int correctiondivisor = 0;
                foreach (ProteinGroup sharedpg in pg.SharedGroupList)
                {
                    correctiondivisor += sharedpg.UniquePsms;
                }
                pg.CorrectedPsms = (((double) pg.UniquePsms/correctiondivisor)*pg.SharedPsms);
            }
        }
        
        // The following three methods can probably be collapsed to traverse the pg list only twice?
        public static void CorrectedSpectralAbundanceFactorNormalizer(ProteomicsExperimentRun per)
        {
            double totalSpectralAbundanceFactors = 0;
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                totalSpectralAbundanceFactors += pg.CorrectedSpectralAbundanceFactor;
            }
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                pg.NormalizedCorrectedSpectralAbundanceFactor = (double) pg.CorrectedSpectralAbundanceFactor/
                                                                totalSpectralAbundanceFactors;
            }
        }

        public static void SpectralAbundanceFactorNormalizer(ProteomicsExperimentRun per)
        {
            double totalSpectralAbundanceFactors = 0;
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                totalSpectralAbundanceFactors += pg.SpectralAbundanceFactor;
            }
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                pg.NormalizedSpectralAbundanceFactor = (double) pg.SpectralAbundanceFactor / totalSpectralAbundanceFactors;
            }
        }

        public static void UniqueSPectralAbundanceFactorNormalizer(ProteomicsExperimentRun per)
        {
            double totalSpectralAbundanceFactors = 0;
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                totalSpectralAbundanceFactors += pg.UniqueSpectralAbundanceFactor;
            }
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                pg.NormalizedUniqueSpectralAbundanceFactor = (double) pg.UniqueSpectralAbundanceFactor/
                                                             totalSpectralAbundanceFactors;
            }
        }

        public static void CalculateIonCurrentIntensities(ProteomicsExperimentRun per)
        {
            /*
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                double summedprecursurintensity;
                double summedmatchedms2intensity;
                double sharedsummedprecursurintensity;
                double sharedsummedmatchedms2intensity;

                foreach (Psm psm in per.PsmList.PeptideSpectraMatchListist)
                {
                    if ((psm.MatchingProteingroupList.Count == 1) && (psm.MatchingProteingroupList.Contains(pg)))
                    {
                        summedprecursurintensity += psm.PrecurserIntensity;
                        summedmatchedms2intensity += psm.MatchingMs2Intensity;
                    }
                    if (psm.MatchingProteingroupList.Contains(pg))
                    {
                        sharedsummedprecursurintensity += psm.PrecurserIntensity;
                        sharedsummedmatchedms2intensity += psm.PrecurserIntensity;
                    }
                    if (psm.MatchingProteingroupList.Contains(pg) && (psm.MatchingProteingroupList.Count > 1))
                    {
                        foreach (ProteinGroup sharedpg in psm.MatchingProteingroupList)
                        {
                            if (!pg.SharedGroupList.Contains(sharedpg))
                            {
                                pg.SharedGroupList.Add(sharedpg);
                            }
                        }
                    }
                }
            }
             */
        }

        // Todo implement cNSAF
        // http://www.ncbi.nlm.nih.gov/pmc/articles/PMC3033667/
        // Problem here where we need to figure out how and what to output, if we whitelist a bunch of stuff should we return
        // a new proteomics experiment, or a subset, I think we should return a list of protein groups with a whitelisted NSAF
    }
}
