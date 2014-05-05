using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Morpheus_Spectral_Counter
{
    public class SpectralCounter
    {
        public List<string> FilesToProcess;
        public List<ProteomicsExperimentRun> Proteomicrunlist = new List<ProteomicsExperimentRun>();
        public string WhiteList { get; set; }
        public bool WhiteListActive { get; set; }
        public bool NSAF { get; set;}
        public bool dNSAF { get; set; }
        public bool uNSAF { get; set; }
        public double ProteinFDR { get; set; }
        public double PeptideFDR { get; set; }
        public Action<int> ReportProgressDelegate { get; set; }


        private void ReportProgress(int percent)
        {
            if (ReportProgressDelegate != null)
                ReportProgressDelegate(percent);
        }

        /*
        public void SummarizeData()
        {
            foreach (string s in FilesToProcess)
            {
                string basefile = s.Split('.')[0];
                //Console.WriteLine("MorpheusSpectralCounter");
                string basefiletoread = basefile;
                ProteomicsExperimentRun proteomicsexperimentrun = Utilities.ImportMorpheusData(basefiletoread);

                proteomicsexperimentrun.OutPutNSAF = NSAF;
                proteomicsexperimentrun.OutputDNSAF = dNSAF;
                proteomicsexperimentrun.OutputUNSAF = uNSAF;

                Utilities.FilterProteomicsExperimentByQValue(proteomicsexperimentrun, ProteinFDR, PeptideFDR);
                PsmToProteinMatcher.MatchPsmsToProteins(proteomicsexperimentrun);
                PsmSummarizer.CalculateUniqueAndSharedPsms(proteomicsexperimentrun);
                PsmSummarizer.CalculateAdjustedPsms(proteomicsexperimentrun);
                PsmSummarizer.CorrectedSpectralAbundanceFactorNormalizer(proteomicsexperimentrun);
                PsmSummarizer.SpectralAbundanceFactorNormalizer(proteomicsexperimentrun);
                PsmSummarizer.UniqueSPectralAbundanceFactorNormalizer(proteomicsexperimentrun);
                //Seperate Single vs multiple Inputs
                if (WhiteListActive)
                {
                    ProteomicsExperimentRun whiteListedProteomicsExperimentRun = new ProteomicsExperimentRun();
                    whiteListedProteomicsExperimentRun =
                        Utilities.FilterProteomicsExperimentByWhiteListForOutput(proteomicsexperimentrun,
                            Utilities.GetWhitelist(WhiteList));
                    Proteomicrunlist.Add(whiteListedProteomicsExperimentRun);
                    ProteomicsExperimentOutputExporter.Export(whiteListedProteomicsExperimentRun);
                }
                else
                {
                    Proteomicrunlist.Add(proteomicsexperimentrun);
                    ProteomicsExperimentOutputExporter.Export(proteomicsexperimentrun);
                }
            }
            ProteomicsExperimentOutputExporter.Export(Proteomicrunlist);
            
        }
         */
        
        public void SummarizeDataFromSummaryFile(MorpheusSummaryFile msf, string outputdirectory)
        {
            
            //instantiate a new Summary Run
            ProteomicsExperimentRunSummary perSummary = new ProteomicsExperimentRunSummary();
            //Populate the Summary Run's PSMs and Protein Groups through PSMs file and ProteinGroupsFile
            perSummary = Utilities.ImportMorpheusDataSummary(msf);
            //Set Flags 
            perSummary.OutPutNSAF = NSAF;
            perSummary.OutputDNSAF = dNSAF;
            perSummary.OutputUNSAF = uNSAF;
            
            //Filter Out Q Values
            Utilities.FilterProteomicsExperimentByQValue(perSummary, ProteinFDR, PeptideFDR);


            //Figure out how to handle decoys if neccesary

            //Populate Protein Groups for each Experiment
            //Use Depreciated Memberwise Clone!
            //This fixes the problem with each proteingroup object populating as the same object into each experiment, and will causes calculation problems
            //Had to switch over to a DeepCloneExtensionMethod as it faithfully copies all the data
            //MemberwiseClone causes some problems with state and induces some NaN values into the CorrectedPeptideCount
            foreach (ProteomicsExperimentRun per in msf.ProteomicsExperimentRunsInSummaryFile)
            {
                foreach (ProteinGroup pg in perSummary.ProteingroupList.Pglist)
                {
                    ProteinGroup ClonedPg = new ProteinGroup();
                    ClonedPg = pg.DeepClone();
                    per.ProteingroupList.Pglist.Add(ClonedPg);
                }
            }

            //Split the PSMS by experimentID and add to the ProteomicsExperimentRuns in the MorpheusSummaryFileObject
            int counter = 0;

            foreach (ProteomicsExperimentRun per in msf.ProteomicsExperimentRunsInSummaryFile)
            {
                foreach (Psm psm in perSummary.PsmList.PeptideSpectraMatchListist)
                {
                    if (Path.GetFileNameWithoutExtension(psm.RawdataFilename) == per.ExperimentId)
                    {
                        per.PsmList.PeptideSpectraMatchListist.Add(psm);

                    }
                }
                //Determine which proteingroups have evidence using Utilities.ProteinGroupsWithEvidence(PSMS in 1 experiment, All ProteinGroups)
                //This returns only protein groups with evidence
                //Set the proteomics experiment run with the return proteingroups that have evidence
                per.ProteingroupList = Utilities.ProteinGroupsWithEvidence(per.PsmList, per.ProteingroupList);
                
                //Transfer Flags for Output
                per.OutPutNSAF = perSummary.OutPutNSAF;
                per.OutputDNSAF = perSummary.OutputDNSAF;
                per.OutputUNSAF = perSummary.OutputUNSAF;
                

                //Match all PSMs from 1 experiment to these protein groups using PsmsToProteinMatcher
                PsmToProteinMatcher.MatchPsmsToProteins(per);

                //Run PSMSummarizer
                PsmSummarizer.CalculateUniqueAndSharedPsms(per);
                //After Calculating Adjusted Psms, NaN propigates
                PsmSummarizer.CalculateAdjustedPsms(per);
                PsmSummarizer.CorrectedSpectralAbundanceFactorNormalizer(per);
                PsmSummarizer.SpectralAbundanceFactorNormalizer(per);
                PsmSummarizer.UniqueSPectralAbundanceFactorNormalizer(per);

                // Whitelisting will not match Decoy's
                if (WhiteListActive)
                {
                    ProteomicsExperimentRun whiteListedProteomicsExperimentRun = new ProteomicsExperimentRun();
                    whiteListedProteomicsExperimentRun = Utilities.FilterProteomicsExperimentByWhiteListForOutput(per, Utilities.GetWhitelist(WhiteList));
                    Proteomicrunlist.Add(whiteListedProteomicsExperimentRun);
                    ProteomicsExperimentOutputExporter.Export(whiteListedProteomicsExperimentRun, outputdirectory);
                }
                else
                {
                    Proteomicrunlist.Add(per);
                    ProteomicsExperimentOutputExporter.Export(per, outputdirectory);
                }
                counter++;
                double doublecounter = counter;
                double numberofruns = msf.ProteomicsExperimentRunsInSummaryFile.Count;
                double percent = (doublecounter/numberofruns)*100;
                int percenttoreport = (int) percent;
                ReportProgress(percenttoreport);
            }
            ProteomicsExperimentOutputExporter.ExportNsafSummary(Proteomicrunlist, outputdirectory);
        }
    }
}
