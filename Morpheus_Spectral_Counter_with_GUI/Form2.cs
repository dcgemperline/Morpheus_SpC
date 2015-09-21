using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Morpheus_Spectral_Counter;
using Morpheus_Spectral_Count_with_GUI;

namespace Morpheus_Spectral_Counter_with_GUI
{
    public partial class Form2 : Form
    {
        MorpheusSummaryFileLabeledForExperiments morpheusSummaryFileLabeled { get; set; }
        private MorpheusSummaryFile morpheusSummaryFile;
        private BindingList<Experiment> experimentlist = new BindingList<Experiment>();
        public ExperimentalDesign ex { get; set; }
        public Form2(MorpheusSummaryFile msf)
        {
            morpheusSummaryFile = msf;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.DataSource()
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //On Load Populate Experimental Designer with Morpheus Summary File
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;


            DataGridViewTextBoxColumn ExperimentIDColumn = new DataGridViewTextBoxColumn();
            ExperimentIDColumn.DataPropertyName = "ExperimentID";
            ExperimentIDColumn.HeaderText = "Experiments";
            DataGridViewTextBoxColumn BioRepIDColumn = new DataGridViewTextBoxColumn();
            BioRepIDColumn.DataPropertyName = "BioRepID";
            BioRepIDColumn.HeaderText = "Bio Rep Label";
            DataGridViewTextBoxColumn TechRepIDColumn = new DataGridViewTextBoxColumn();
            TechRepIDColumn.DataPropertyName = "TechRepID";
            TechRepIDColumn.HeaderText = "Tech Rep Label";

            dataGridView1.Columns.Add(ExperimentIDColumn);
            dataGridView1.Columns.Add(BioRepIDColumn);
            dataGridView1.Columns.Add(TechRepIDColumn);


            foreach (ProteomicsExperimentRun per in morpheusSummaryFile.ProteomicsExperimentRunsInSummaryFile)
            {
                experimentlist.Add(new Experiment(per.ExperimentId));
            }
            dataGridView1.DataSource = experimentlist;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dataGridView1.Columns[0].ReadOnly = true;





        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExperimentalDesign exDesign = new ExperimentalDesign(experimentlist.ToList());
            morpheusSummaryFileLabeled = new MorpheusSummaryFileLabeledForExperiments(morpheusSummaryFile, exDesign);
        }
    }
}
