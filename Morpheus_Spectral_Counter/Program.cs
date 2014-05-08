using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    class Program
    {
        //reworking multiple file parsimony
        static void Main(string[] args)
        {
            //List<string> fileListToProcess = new List<string>();
            //string line;
            //System.IO.StreamReader file = new StreamReader(args[0]);
            //while ((line = file.ReadLine()) != null)
            //{
            //    fileListToProcess.Add(line);
            //}
            //file.Close();

            //foreach (string s in fileListToProcess)
            //{
                //Console.WriteLine("MorpheusSpectralCounter");
                /*
                string basefiletoread = s;
                ProteomicsExperimentRun proteomicsexperimentrun = Utilities.ImportMorpheusData(basefiletoread);
                Utilities.FilterProteomicsExperimentByQValue(proteomicsexperimentrun, 1.0, 1.0);
                PsmToProteinMatcher.MatchPsmsToProteins(proteomicsexperimentrun);
                PsmSummarizer psmsum = new PsmSummarizer();
                psmsum.CalculateUniqueAndSharedPsms(proteomicsexperimentrun);
                psmsum.CalculateAdjustedPsms(proteomicsexperimentrun);
                psmsum.CorrectedSpectralAbundanceFactorNormalizer(proteomicsexperimentrun);
                ProteomicsExperimentRun whiteListedProteomicsExperimentRun = new ProteomicsExperimentRun();
                whiteListedProteomicsExperimentRun = Utilities.FilterProteomicsExperimentByWhiteListForOutput(proteomicsexperimentrun, Utilities.GetWhitelist("whitelist"));
                ProteomicsExperimentOutputExporter.Export(whiteListedProteomicsExperimentRun);
                 */
            //}
            
        }
    }
}
