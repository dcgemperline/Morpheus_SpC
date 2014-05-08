using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    static class Utilities
    {
        public static ProteomicsExperimentRun ImportMorpheusData(string baseFilename)
        {
            string psmFileToOpen = baseFilename + ".PSMs.tsv";
            string proteinGroupFileToOpen = baseFilename + ".protein_groups.tsv";
            ProteomicsExperimentRun per = new ProteomicsExperimentRun();
            System.IO.StreamReader file = new System.IO.StreamReader(psmFileToOpen);
            //Strip Header (remove first line)
            file.ReadLine();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string [] dataline = line.Split('\t');
                Psm psmline = new Psm
                {
                    MatchingIntensity = Double.Parse(dataline[23]),
                    MissedCleavages = Int32.Parse(dataline[16]),
                    PeptideBaseSequence = dataline[12],
                    PrecurserIntensity = Double.Parse(dataline[6]),
                    PrecurserMassOverCharge = Double.Parse(dataline[5]),
                    PrecurserCharge = Int32.Parse(dataline[7]),
                    PrecurserMass = Double.Parse(dataline[8]),
                    ProteinDescription = dataline[13],
                    QValue = Double.Parse(dataline[30]),
                    RawdataFilename = dataline[0],
                    RetentionTime = Double.Parse(dataline[4]),
                    SpectrumNum = Int32.Parse(dataline[1]),
                    Target = Boolean.Parse(dataline[26])
                };
                per.PsmList.PeptideSpectraMatchListist.Add(psmline);
            }
            file.Close();
            System.IO.StreamReader file2 = new System.IO.StreamReader(proteinGroupFileToOpen);
            //Strip header (Remove first line)
            file2.ReadLine();
            string[] splitOn = {"; "};
            while ((line = file2.ReadLine()) != null)
            {
                string [] dataline = line.Split('\t');
                ProteinGroup pg = new ProteinGroup { ProteingroupId = dataline[0] };
                pg.QValue = Double.Parse(dataline[14]);
                string[] proteins = dataline[0].Split(splitOn, StringSplitOptions.None);
                string[] proteinseq = dataline[1].Split(splitOn, StringSplitOptions.None);
                string[] seqcoverages = dataline[8].Split(splitOn, StringSplitOptions.None);
                for (int index = 0; index < proteins.Length; index++)
                {
                    Protein p = new Protein
                    {
                        ProteinId = proteins[index],
                        Sequence = proteinseq[index],
                        SequenceCoverage = double.Parse(seqcoverages[index])
                    };
                    pg.ProteingroupList.Add(p);
                }
                per.ProteingroupList.Pglist.Add(pg);
            }
            file.Close();
            per.ExperimentId = baseFilename;
            return per;
        }

        public static ProteomicsExperimentRunSummary ImportMorpheusDataSummary(MorpheusSummaryFile msf)
        {
            // Select the Summary File to get all the files that were run together
            string baseDirectory = msf.SummaryFilePath;
            string psmFileToOpen = Path.Combine(baseDirectory, "PSMs.tsv");
            string proteinGroupFileToOpen = Path.Combine(baseDirectory, "protein_groups.tsv");

            if (msf.NumberOfFilesInSummaryFile == 1)
            {
                psmFileToOpen = Path.Combine(baseDirectory, (msf.ProteomicsExperimentRunsInSummaryFile.ElementAt(0).ExperimentId + ".PSMs.tsv"));
                proteinGroupFileToOpen = Path.Combine(baseDirectory, (msf.ProteomicsExperimentRunsInSummaryFile.ElementAt(0).ExperimentId + ".protein_groups.tsv"));
            }

            ProteomicsExperimentRunSummary persummary = new ProteomicsExperimentRunSummary();
            System.IO.StreamReader file = new System.IO.StreamReader(psmFileToOpen);
            //Strip Header (remove first line)
            file.ReadLine();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] dataline = line.Split('\t');
                /*
                Psm psmline = new Psm
                {
                    MatchingIntensity = Double.Parse(dataline[23]),
                    MissedCleavages = Int32.Parse(dataline[16]),
                    PeptideBaseSequence = dataline[12],
                    PrecurserIntensity = Double.Parse(dataline[6]),
                    PrecurserMassOverCharge = Double.Parse(dataline[5]),
                    PrecurserCharge = Int32.Parse(dataline[7]),
                    PrecurserMass = Double.Parse(dataline[8]),
                    ProteinDescription = dataline[13],
                    QValue = Double.Parse(dataline[30]),
                    RawdataFilename = dataline[0],
                    RetentionTime = Double.Parse(dataline[4]),
                    SpectrumNum = Int32.Parse(dataline[1]),
                    Target = Boolean.Parse(dataline[26])
                };
                 */
                Psm psmline = new Psm();
                
                psmline.MatchingIntensity = Double.Parse(dataline[23]);
                psmline.MissedCleavages = Int32.Parse(dataline[16]);
                psmline.PeptideBaseSequence = dataline[12];
                psmline.PrecurserIntensity = Double.Parse(dataline[6]);
                psmline.PrecurserMassOverCharge = Double.Parse(dataline[5]);
                psmline.PrecurserCharge = Int32.Parse(dataline[7]);
                psmline.PrecurserMass = Double.Parse(dataline[8]);
                psmline.ProteinDescription = dataline[13];
                psmline.QValue = Double.Parse(dataline[30]);
                psmline.RawdataFilename = dataline[0];
                psmline.RetentionTime = Double.Parse(dataline[4]);
                psmline.SpectrumNum = Int32.Parse(dataline[1]);
                psmline.Target = Boolean.Parse(dataline[26]);






                persummary.PsmList.PeptideSpectraMatchListist.Add(psmline);
            }
            file.Close();
            System.IO.StreamReader file2 = new System.IO.StreamReader(proteinGroupFileToOpen);
            //Strip header (Remove first line)
            file2.ReadLine();
            string[] splitOn = { "; " };
            while ((line = file2.ReadLine()) != null)
            {
                string[] dataline = line.Split('\t');
                ProteinGroup pg = new ProteinGroup { ProteingroupId = dataline[0] };
                pg.QValue = Double.Parse(dataline[14]);
                string[] proteins = dataline[0].Split(splitOn, StringSplitOptions.None);
                string[] proteinseq = dataline[1].Split(splitOn, StringSplitOptions.None);
                string[] seqcoverages = dataline[8].Split(splitOn, StringSplitOptions.None);
                for (int index = 0; index < proteins.Length; index++)
                {
                    Protein p = new Protein
                    {
                        ProteinId = proteins[index],
                        Sequence = proteinseq[index],
                        SequenceCoverage = double.Parse(seqcoverages[index])
                    };
                    pg.ProteingroupList.Add(p);
                }
                persummary.ProteingroupList.Pglist.Add(pg);
            }
            file.Close();
            return persummary;    
        }
        
        public static List<T> FilterByQValue<T>(List<T> list, double threshold) where T : IQValue
        {
            List<T> returnlist = new List<T>();
            foreach (T t in list)
            {
                if (t.QValue < threshold)
                {
                    returnlist.Add(t);
                }
            }
            return returnlist;
        }

        public static void FilterProteomicsExperimentByQValue(ProteomicsExperimentRun per, double qValueThresholdProtein, double qValueThresholdPeptide)
        {
            per.ProteingroupList.QValueThreshold = qValueThresholdProtein;
            per.PsmList.QValueThreshold = qValueThresholdPeptide;
            per.ProteingroupList.FilterByQValue();
            per.PsmList.FilterByQValue();
        }

        public static ProteomicsExperimentRun FilterProteomicsExperimentByWhiteListForOutput(ProteomicsExperimentRun per, Dictionary<string,string> whitelist)
        {
            ProteomicsExperimentRun perToReturn = new ProteomicsExperimentRun();
            perToReturn.ExperimentId = per.ExperimentId;
            perToReturn.OutPutNSAF = per.OutPutNSAF;
            perToReturn.OutputDNSAF = per.OutputDNSAF;
            perToReturn.OutputUNSAF = per.OutputUNSAF;
            Dictionary<string,string> namelookup = new Dictionary<string, string>();
            foreach (ProteinGroup pg in per.ProteingroupList.Pglist)
            {
                foreach (KeyValuePair<string,string> kvpair in whitelist)
                {
                    if (pg.ProteingroupId.Contains(kvpair.Key) && !pg.ProteingroupId.Contains("DECOY_"))
                    {
                        perToReturn.ProteingroupList.Pglist.Add(pg);
                        pg.WhiteListName = kvpair.Value;
                    }
                }
            }
            perToReturn.Whitelisted = true;
            perToReturn.WhitelistDictionary = whitelist;
            return perToReturn;
        }

        public static Dictionary<string,string> GetWhitelist(string filename)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            char splitON = ',';
            string line;
            Dictionary<string,string> DictionaryToReturn = new Dictionary<string, string>();
            while ((line = file.ReadLine()) != null)
            {
                string[] dataline = line.Split(splitON);
                DictionaryToReturn.Add(dataline[0],dataline[1]);
            }
            file.Close();
            return DictionaryToReturn;
        }

        public static string LookupWhiteListedBasedonId(Dictionary<string, string> lookupDictionary , string stringtolookup )
        {
            string findthis = stringtolookup;
            Dictionary<string, string> lookhere = lookupDictionary;
            string stringtoreturn = "";
            foreach (KeyValuePair<string, string>  kvpair in lookhere)
            {
                //if (kvpair.Key.Contains(stringtolookup))
                if(stringtolookup.Contains(kvpair.Key))
                {
                    stringtoreturn = kvpair.Value;
                }
                
            }
            return stringtoreturn;
        }

        public static ProteinGroupList ProteinGroupsWithEvidence(PsmList psmList, ProteinGroupList proteinGroupList)
        {
            ProteinGroupList returnProteinGroupList = new ProteinGroupList();
            PsmList psmsContributingToEvidence = new PsmList();

            //If a PSM only matches 1 Protein Group in the ProteinGroupList Then add this proteingroup to the return proteingroupList

            foreach (Psm psm in psmList.PeptideSpectraMatchListist)
            {
                int counter = 0;
                foreach (ProteinGroup pg in proteinGroupList.Pglist)
                {
                    Boolean addedPSM = false;
                    foreach (Protein p in pg.ProteingroupList)
                    {
                        if (p.Sequence.Contains(psm.PeptideBaseSequence))
                        {
                            addedPSM = true;
                        }
                    }
                    if (addedPSM)
                    {
                        counter++;
                    }
                }
                if (counter == 1)
                {
                    psmsContributingToEvidence.PeptideSpectraMatchListist.Add(psm);
                }
            }

            foreach (ProteinGroup pg in proteinGroupList.Pglist)
            {
                Boolean addedGroup = false;
                foreach (Protein p in pg.ProteingroupList)
                {
                    foreach (Psm psm in psmsContributingToEvidence.PeptideSpectraMatchListist)
                    {
                        if (p.Sequence.Contains(psm.PeptideBaseSequence))
                        {
                            addedGroup = true;
                        }
                    }
                }
                if (addedGroup)
                {
                   returnProteinGroupList.Pglist.Add(pg); 
                }
            }
            return returnProteinGroupList;
        }

        public static double CalculatePercentofPeptidesWithMissedCleavages(ProteomicsExperimentRun per)
        {
            int psmsWithMissedCleavagesCounter = 0;
            foreach (Psm psm in per.PsmList.PeptideSpectraMatchListist)
            {
                if (psm.MissedCleavages > 0)
                {
                    psmsWithMissedCleavagesCounter++;
                }
            }
            return ((double)psmsWithMissedCleavagesCounter / per.PsmList.PeptideSpectraMatchListist.Count) * 100;
        }

        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
