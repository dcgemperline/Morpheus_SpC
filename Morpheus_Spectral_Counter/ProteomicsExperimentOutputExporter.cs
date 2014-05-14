﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public static class ProteomicsExperimentOutputExporter
    {
        private const string delimiter = "\t";
        private const char chardelimiter = '\t';
        private const string outputFileLabel = "_NSAF_Sumary.tsv";
        private const string outputSummaryFileLabel = "NSAF_Summary.tsv";

        public static void Export(ProteomicsExperimentRun per, string outputdirectory)
        {
            string outputfile = Path.Combine(outputdirectory, (per.ExperimentId + outputFileLabel));
            using (StreamWriter sw = new StreamWriter(outputfile))
            {
                sw.WriteLine(per.ExperimentId);
                StringBuilder header = new StringBuilder();

                header.Append("ProteinGroup" + delimiter);
                if (per.Whitelisted)
                {
                    header.Append("WhiteListID" + delimiter);
                }
                //header.Append("AverageSequenceCoverage" + delimiter);
                header.Append("UniquePsms" + delimiter);
                header.Append("SharedPsms" + delimiter);
                header.Append("TotalPsms" + delimiter);
                header.Append("TotalCorrectedPsms" + delimiter);
                if (per.OutputUNSAF)
                {
                    header.Append("uNSAF" + delimiter);
                }
                if (per.OutPutNSAF)
                {
                    header.Append("NSAF" + delimiter);
                }
                if (per.OutputDNSAF)
                {
                    header.Append("dNSAF" + delimiter);
                }
                sw.WriteLine(header);
                //sw.WriteLine("ProteinGroup,WhiteListId,AverageSequenceCoverage,UniquePsms,SharedPsms,TotalPsms,TotalCorrectedPsms,UniqueNSAF,NSAF,CorrectedNSAF");
                foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(pg.ProteingroupId);
                    sb.Append(delimiter);
                    if (per.Whitelisted)
                    {
                        sb.Append(pg.WhiteListName);
                        sb.Append(delimiter);
                    }
                    //sb.Append(pg.AverageSequenceCoverage);
                    //sb.Append(delimiter);
                    sb.Append(pg.UniquePsms);
                    sb.Append(delimiter);
                    sb.Append(pg.SharedPsms);
                    sb.Append(delimiter);
                    sb.Append(pg.TotalPsms);
                    sb.Append(delimiter);
                    sb.Append(pg.TotalCorrectedPsms);
                    sb.Append(delimiter);
                    if (per.OutputUNSAF)
                    {
                        sb.Append(pg.NormalizedUniqueSpectralAbundanceFactor);
                        sb.Append(delimiter);
                    }
                    if (per.OutPutNSAF)
                    {
                        sb.Append(pg.NormalizedSpectralAbundanceFactor);
                        sb.Append(delimiter);
                    }
                    if (per.OutputDNSAF)
                    {
                        sb.Append(pg.NormalizedCorrectedSpectralAbundanceFactor);
                        sb.Append(delimiter);
                    }
                    sw.WriteLine(sb);
                }
            }
            
        }

        public static void Export(List<ProteomicsExperimentRun> perlist, string outputdirectory)
        {
            //OpenFileForWriting

            
            List<String> uniqueProteinGroups = new List<String>();

            StringBuilder proteomicExperimentLabels = new StringBuilder();
            StringBuilder header = new StringBuilder();

            //Get spacing for proteomics experimentlabel
            foreach (ProteomicsExperimentRun per in perlist)
            {
                header.Append("ProteinGroup" + delimiter);
                if (per.Whitelisted)
                {
                    header.Append("WhiteListID" + delimiter);
                }
                //header.Append("AverageSequenceCoverage" + delimiter);
                header.Append("UniquePsms" + delimiter);
                header.Append("SharedPsms" + delimiter);
                header.Append("TotalPsms" + delimiter);
                header.Append("TotalCorrectedPsms" + delimiter);
                if (per.OutputUNSAF)
                {
                    header.Append("uNSAF" + delimiter);
                }
                if (per.OutPutNSAF)
                {
                    header.Append("NSAF" + delimiter);
                }
                if (per.OutputDNSAF)
                {
                    header.Append("dNSAF" + delimiter);
                }
                foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
                {
                    if (!uniqueProteinGroups.Contains(pg.ProteingroupId))
                    {
                        uniqueProteinGroups.Add(pg.ProteingroupId);
                    }
                }
            }
            
            //This spaces the ExperimentHeader Appropriately based on the output selected, uNSAF, NSAF, dNSAF etc
            int spacing = ((header.ToString().Split(chardelimiter).Count()-1)/perlist.Count);

            foreach (ProteomicsExperimentRun per in perlist)
            {
                proteomicExperimentLabels.Append(per.ExperimentId);
                for (int i = 0; i < spacing; i++)
                {
                    proteomicExperimentLabels.Append(delimiter);
                }
            }

            string outputfile = Path.Combine(outputdirectory, outputSummaryFileLabel);
            using (StreamWriter sw2 = new StreamWriter(outputfile))
            {
                sw2.WriteLine(proteomicExperimentLabels);
                sw2.WriteLine(header);
                foreach (String pgId in uniqueProteinGroups)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ProteomicsExperimentRun per in perlist)
                    {
                        bool isfound = false;
                        foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
                        {
                            if (pg.ProteingroupId == pgId)
                            {
                                isfound = true;
                                ProteinGroup proteinGroupToWrite = pg;
                                sb.Append(proteinGroupToWrite.ProteingroupId);
                                sb.Append(delimiter);
                                if (per.Whitelisted)
                                {
                                    sb.Append(proteinGroupToWrite.WhiteListName);
                                    sb.Append(delimiter);
                                }
                                //sb.Append(proteinGroupToWrite.AverageSequenceCoverage);
                                //sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.UniquePsms);
                                sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.SharedPsms);
                                sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.TotalPsms);
                                sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.TotalCorrectedPsms);
                                sb.Append(delimiter);
                                if (per.OutputUNSAF)
                                {
                                    sb.Append(proteinGroupToWrite.NormalizedUniqueSpectralAbundanceFactor);
                                    sb.Append(delimiter);
                                }
                                if (per.OutPutNSAF)
                                {
                                    sb.Append(proteinGroupToWrite.NormalizedSpectralAbundanceFactor);
                                    sb.Append(delimiter);
                                }
                                if (per.OutputDNSAF)
                                {
                                    sb.Append(proteinGroupToWrite.NormalizedCorrectedSpectralAbundanceFactor);
                                    sb.Append(delimiter);
                                }
                            }
                        }
                        if (isfound == false)
                            {
                                const string notfound = "0";
                                //Write Zeros or #NA for the data output for that line.
                                sb.Append(pgId);
                                sb.Append(delimiter);
                                if (per.Whitelisted)
                                {
                                    // Some special logic if whitelist isnt found needs to go here. Need to handle this somehow
                                    string stringtolookup = pgId.Split('.')[0];

                                    // DJB edit
                                    //string outputString = "";
                                    //per.WhitelistDictionary.TryGetValue(stringtolookup, out outputString);

                                    // DJB Edit


                                    sb.Append(Utilities.LookupWhiteListedBasedonId(per.WhitelistDictionary, stringtolookup));
                                    //sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                                
                                //sb.Append(notfound);
                                //sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                if (per.OutputUNSAF)
                                {
                                    sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                                if (per.OutPutNSAF)
                                {
                                    sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                                if (per.OutputDNSAF)
                                {
                                    sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                            }
                    }
                    sw2.WriteLine(sb); //Write the data if we have it found based on the flags set
                } //EndLoopingthroughUniqueIDs
            } //Endfileoutput
        }

        public static void ExportNsafSummary(List<ProteomicsExperimentRun> perlist, string outputdirectory)
        {
            //OpenFileForWriting


            List<String> uniqueProteinGroups = new List<String>();
            StringBuilder proteomicExperimentLabels = new StringBuilder();
            StringBuilder header = new StringBuilder();

            //Get spacing for proteomics experimentlabel
            foreach (ProteomicsExperimentRun per in perlist)
            {
                header.Append("ProteinGroup" + delimiter);
                if (per.Whitelisted)
                {
                    header.Append("WhiteListID" + delimiter);
                }
                //header.Append("AverageSequenceCoverage" + delimiter);
                header.Append("UniquePsms" + delimiter);
                header.Append("SharedPsms" + delimiter);
                header.Append("TotalPsms" + delimiter);
                header.Append("TotalCorrectedPsms" + delimiter);
                if (per.OutputUNSAF)
                {
                    header.Append("uNSAF" + delimiter);
                }
                if (per.OutPutNSAF)
                {
                    header.Append("NSAF" + delimiter);
                }
                if (per.OutputDNSAF)
                {
                    header.Append("dNSAF" + delimiter);
                }
                foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
                {
                    if (!uniqueProteinGroups.Contains(pg.ProteingroupId))
                    {
                        uniqueProteinGroups.Add(pg.ProteingroupId);
                    }
                }
            }
            
            //This spaces the ExperimentHeader Appropriately based on the output selected, uNSAF, NSAF, dNSAF etc
            int spacing = ((header.ToString().Split(chardelimiter).Count()-1)/perlist.Count);

            foreach (ProteomicsExperimentRun per in perlist)
            {
                proteomicExperimentLabels.Append(per.ExperimentId);
                for (int i = 0; i < spacing; i++)
                {
                    proteomicExperimentLabels.Append(delimiter);
                }
            }

            string outputfile = Path.Combine(outputdirectory, outputSummaryFileLabel);
            using (StreamWriter sw2 = new StreamWriter(outputfile))
            {
                sw2.WriteLine(proteomicExperimentLabels);
                sw2.WriteLine(header);
                foreach (String pgId in uniqueProteinGroups)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ProteomicsExperimentRun per in perlist)
                    {
                        bool isfound = false;
                        foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
                        {
                            if (pg.ProteingroupId == pgId)
                            {
                                isfound = true;
                                ProteinGroup proteinGroupToWrite = pg;
                                sb.Append(proteinGroupToWrite.ProteingroupId);
                                sb.Append(delimiter);
                                if (per.Whitelisted)
                                {
                                    sb.Append(proteinGroupToWrite.WhiteListName);
                                    sb.Append(delimiter);
                                }
                                //sb.Append(proteinGroupToWrite.AverageSequenceCoverage);
                                //sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.UniquePsms);
                                sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.SharedPsms);
                                sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.TotalPsms);
                                sb.Append(delimiter);
                                sb.Append(proteinGroupToWrite.TotalCorrectedPsms);
                                sb.Append(delimiter);
                                if (per.OutputUNSAF)
                                {
                                    sb.Append(proteinGroupToWrite.NormalizedUniqueSpectralAbundanceFactor);
                                    sb.Append(delimiter);
                                }
                                if (per.OutPutNSAF)
                                {
                                    sb.Append(proteinGroupToWrite.NormalizedSpectralAbundanceFactor);
                                    sb.Append(delimiter);
                                }
                                if (per.OutputDNSAF)
                                {
                                    sb.Append(proteinGroupToWrite.NormalizedCorrectedSpectralAbundanceFactor);
                                    sb.Append(delimiter);
                                }
                            }
                        }
                        if (isfound == false)
                            {
                                const string notfound = "0";
                                //Write Zeros or #NA for the data output for that line.
                                sb.Append(pgId);
                                sb.Append(delimiter);
                                if (per.Whitelisted)
                                {
                                    // Some special logic if whitelist isnt found needs to go here. Need to handle this somehow
                                    string stringtolookup = pgId.Split('.')[0];
                                    sb.Append(Utilities.LookupWhiteListedBasedonId(per.WhitelistDictionary, stringtolookup));
                                    //sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                                
                                //sb.Append(notfound);
                                //sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                sb.Append(notfound);
                                sb.Append(delimiter);
                                if (per.OutputUNSAF)
                                {
                                    sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                                if (per.OutPutNSAF)
                                {
                                    sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                                if (per.OutputDNSAF)
                                {
                                    sb.Append(notfound);
                                    sb.Append(delimiter);
                                }
                            }
                    }
                    sw2.WriteLine(sb); //Write the data if we have it found based on the flags set
                } //EndLoopingthroughUniqueIDs
            } //Endfileoutput
        }
    }
}
