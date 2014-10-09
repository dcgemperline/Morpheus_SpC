using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Morpheus_Spectral_Counter
{
    public class MorpheusSummaryFile
    {
        public string SummaryFileIdentifier { get; set; }
        public string SummaryFilePath { get; set; }
        public List<ProteomicsExperimentRun> ProteomicsExperimentRunsInSummaryFile = new List<ProteomicsExperimentRun>();
        public List<ProteomicsExperimentRunBioAndTechRepLabeled> ProteomicsExperimentRunsInSummaryFileBioandTechRepLabeled = new List<ProteomicsExperimentRunBioAndTechRepLabeled>(); 
        public int NumberOfFilesInSummaryFile { get { return ProteomicsExperimentRunsInSummaryFile.Count(); } }
        //Ctor
        public MorpheusSummaryFile(string filename)
        {
            string summaryFileToOpen = filename;
            SummaryFilePath = Path.GetDirectoryName(filename);
            System.IO.StreamReader file = new System.IO.StreamReader(summaryFileToOpen);
            //Strip Header (remove first line)
            file.ReadLine();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] dataline = line.Split('\t');
                if (dataline[0] != "AGGREGATE")
                {
                    ProteomicsExperimentRun per = new ProteomicsExperimentRun();
                    per.RawFileName = Path.GetFileName(dataline[0]);
                    per.ExperimentId = Path.GetFileNameWithoutExtension(dataline[0]);
                    /*
                    if (per.RawFileName.Contains("*"))
                    {
                        per.RawFileName = per.RawFileName.Replace("*", "");
                    }
                    if (per.ExperimentId.Contains("*"))
                    {
                        per.ExperimentId = per.ExperimentId.Replace("*", "");
                    }
                     */

                    ProteomicsExperimentRunsInSummaryFile.Add(per);
                }
                
            }
            file.Close();
        }
    }
}
