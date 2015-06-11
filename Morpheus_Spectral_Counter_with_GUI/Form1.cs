using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Morpheus_Spectral_Counter;
using Morpheus_Spectral_Counter_with_GUI;

namespace Morpheus_Spectral_Count_with_GUI
{
    public partial class Form1 : Form
    {
        //reworking multifile parsimony
        public double PeptideFalseDiscoveryRate { get { return (double) numericUpDown1.Value; } }
        public double ProteinFalseDiscoveryRate { get { return (double) numericUpDown2.Value; } }
        //public List<string> FilesToProcess { get; set; }
        public string AnalysisDirectory { get; set; }
        public string SummaryFileToProcess { get; set; }
        //public List<string> WhiteLists { get; set; }
        public string WhiteList { get; set; }
        public bool CalclculateUNormalizedSpectralAbundanceFactor { get; set; }
        public bool CalculateNormalizedSPectralAbundanceFactor { get; set; }
        public bool CalculateDNormalizedSpectralAbundanceFactor { get; set; }
        public bool AllAnalysesFilesPresent { get; set; }
        private const string SummaryFile = "summary.tsv";
        private string PsmFile = "PSMs.tsv";
        private string ProteinGroupFile = "protein_groups.tsv";
        public string Outputdirectory { get; set; }
        public string DefaultOutputDirectory { get; set; }
        MorpheusSummaryFile MsfMorpheusSummaryFile { get; set; }


        public Form1()
        {
            InitializeComponent();
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            button5.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            const string filefilter = "Morpheus Summary File | " + SummaryFile;
            SummaryFileToProcess = FileSelector(filefilter, textBox4);
            if (SummaryFileToProcess != "")
            {
                MsfMorpheusSummaryFile = new MorpheusSummaryFile(SummaryFileToProcess);
                DefaultOutputDirectory = Path.GetDirectoryName(SummaryFileToProcess);
                Outputdirectory = DefaultOutputDirectory;
                textBox3.AppendText(Outputdirectory);
                textBox1.Clear();
                StringBuilder sb = new StringBuilder();
                foreach (ProteomicsExperimentRun per in MsfMorpheusSummaryFile.ProteomicsExperimentRunsInSummaryFile)
                {
                    sb.Append(per.ExperimentId);
                    sb.AppendLine();
                }

                textBox1.BackColor = System.Drawing.Color.White;
                textBox1.AppendText(sb.ToString());
                button5.Enabled = true;
                button3.Enabled = true;
                AnalysisDirectory = Path.GetDirectoryName(SummaryFileToProcess);
            }

            bool summaryfile = false;
            bool psmfile = false;
            bool proteingroupfile = false;
            // Null Refernce Check to prevent null argument exceptions 
            if (MsfMorpheusSummaryFile != null)
            {
                if (MsfMorpheusSummaryFile.NumberOfFilesInSummaryFile == 1)
                {
                    ProteinGroupFile = MsfMorpheusSummaryFile.ProteomicsExperimentRunsInSummaryFile.ElementAt(0).ExperimentId + ".protein_groups.tsv";
                    PsmFile = MsfMorpheusSummaryFile.ProteomicsExperimentRunsInSummaryFile.ElementAt(0).ExperimentId + ".PSMs.tsv";
                }
                else
                {
                    PsmFile = "PSMs.tsv";
                    ProteinGroupFile = "protein_groups.tsv";
                }

                if (AnalysisDirectory != "")
                {
                    if (FileExists(AnalysisDirectory, SummaryFile))
                    {
                        label6.ForeColor = System.Drawing.Color.Green;
                        summaryfile = true;
                    }
                    else
                    {
                        label6.ForeColor = System.Drawing.Color.Red;
                        summaryfile = false;
                    }
                    if (FileExists(AnalysisDirectory, PsmFile))
                    {
                        label7.ForeColor = System.Drawing.Color.Green;
                        psmfile = true;
                    }
                    else
                    {
                        label7.ForeColor = System.Drawing.Color.Red;
                        psmfile = false;
                    }
                    if (FileExists(AnalysisDirectory, ProteinGroupFile))
                    {
                        label8.ForeColor = System.Drawing.Color.Green;
                        proteingroupfile = true;
                    }
                    else
                    {
                        label8.ForeColor = System.Drawing.Color.Red;
                        proteingroupfile = false;
                    }

                }

                if (AnalysisDirectory != "" && summaryfile && psmfile && proteingroupfile)
                {
                    button3.Enabled = true;
                }
                else
                {
                    button3.Enabled = false;
                }
            }
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            const string whitelistfilter = "WhiteList | *.csv";
            WhiteList = FileSelector(whitelistfilter, textBox2);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = checkBox1.Checked;
            if (button2.Enabled)
            {
                textBox2.BackColor = System.Drawing.Color.White;
            }
            else
            {
                textBox2.BackColor = Control.DefaultBackColor;
                textBox2.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Start BackgroundWorker to Report to ProgressBar
            backgroundWorker1.RunWorkerAsync();
            //Lock UI
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
        }

        public static string FileSelector(string fileFilter, TextBox textbox)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = fileFilter;
            ofd.Multiselect = false;
            DialogResult dr = ofd.ShowDialog();
            string file = "";
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textbox.Clear();
                file = ofd.FileName;
                textbox.AppendText(file);
            }
            return file;
        }

        public static string DirectorySelector(TextBox textbox)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            string returnstring = "";
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textbox.Clear();
                returnstring = fbd.SelectedPath;
                textbox.AppendText(returnstring);
            }
            return returnstring;
        }

        public string OutputDirectorySelector(TextBox textbox)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = AnalysisDirectory;
            DialogResult dr = fbd.ShowDialog();
            string returnstring = "";
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textbox.Clear();
                returnstring = fbd.SelectedPath;
                textbox.AppendText(returnstring);
            }
            return returnstring;
        }




        private bool FileExists(string rootpath, string filename)
        {
            if (File.Exists(Path.Combine(rootpath, filename)))
            {
                return true;
            } 
            return false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CalculateNormalizedSPectralAbundanceFactor = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CalculateDNormalizedSpectralAbundanceFactor = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CalclculateUNormalizedSpectralAbundanceFactor = checkBox4.Checked;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Outputdirectory = textBox3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Outputdirectory = OutputDirectorySelector(textBox3);


        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Process Data
            SpectralCounter sp = new SpectralCounter();
            sp.WhiteListActive = checkBox1.Checked;
            sp.WhiteList = WhiteList;
            sp.ReportProgressDelegate = backgroundWorker1.ReportProgress;
            sp.PeptideFDR = PeptideFalseDiscoveryRate;
            sp.ProteinFDR = ProteinFalseDiscoveryRate;
            MorpheusSummaryFile msf = new MorpheusSummaryFile(SummaryFileToProcess);
            sp.ProteinFDR = ProteinFalseDiscoveryRate;
            sp.PeptideFDR = PeptideFalseDiscoveryRate;
            sp.NSAF = CalculateNormalizedSPectralAbundanceFactor;
            sp.dNSAF = CalculateDNormalizedSpectralAbundanceFactor;
            sp.uNSAF = CalclculateUNormalizedSpectralAbundanceFactor;
            sp.SummarizeDataFromSummaryFile(msf, Outputdirectory);
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Lock UI
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            button3.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2(MsfMorpheusSummaryFile);
            frm2.Show();
            //Set labeledSummaryFile up as an object in form1?
            //frm2.morpheusSummaryFileLabeled;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
