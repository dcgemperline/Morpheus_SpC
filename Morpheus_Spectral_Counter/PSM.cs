using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class Psm : IQValue
    {
        public string RawdataFilename { get; set; }
        public int SpectrumNum { get; set; }
        public double RetentionTime { get; set; }
        public double PrecurserMassOverCharge { get; set; }
        public int PrecurserCharge { get; set; }
        public double PrecurserMass { get; set; }
        public double PrecurserIntensity { get; set; }
        public string PeptideBaseSequence { get; set; } // convert i to l //DCGEDIT not neccesary as morpheus doesnt report peptides with I to L but reports as I or L based on fasta sequence
        public string ProteinDescription { get; set; }
        public int MissedCleavages { get; set; }
        public double MatchingIntensity { get; set; }
        public double QValue { get; set; }
        public bool Target { get; set; }

        public bool Decoy
        {
            get { return !Target; }
        }

        public List<ProteinGroup> MatchingProteingroupList = new List<ProteinGroup>();
        public List<Protein> MatchingProteinList = new List<Protein>();

        public int NumberOfProteinsMatching
        {
            get { return MatchingProteinList.Count; }
        }

        public int NumberOfProteingroupsMatching
        {
            get { return MatchingProteingroupList.Count; }
        }

        public double ProteinCorrectionFactor { get; set; }
        public double ProteinGroupCorrectionFactor { get; set; }
    }
}
