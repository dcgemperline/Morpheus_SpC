using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    [Serializable]
    public class Psm : IQValue
    {
        public string RawdataFilename { get; set; }
        public int SpectrumNum { get; set; }
        public double RetentionTime { get; set; }
        public double PrecurserMassOverCharge { get; set; }
        public int PrecurserCharge { get; set; }
        public double PrecurserMass { get; set; }
        public double PrecurserIntensity { get; set; }
        public double MatchingMs2Intensity { get; set; }
        public string PeptideBaseSequence { get; set; } // convert i to l //DCGEDIT not neccesary as morpheus doesnt report peptides with I to L but reports as I or L based on fasta sequence
        public string ProteinDescription { get; set; }
        public int MissedCleavages { get; set; }
        public double MatchingIntensity { get; set; }
        public double QValue { get; set; }
        public bool Target { get; set; }
        public double ProteinCorrectionFactor { get; set; }
        public double ProteinGroupCorrectionFactor { get; set; }
        public List<ProteinGroup> MatchingProteingroupList = new List<ProteinGroup>();
        public List<Protein> MatchingProteinList = new List<Protein>();

        public bool Decoy
        {
            get { return !Target; }
        }

        public Psm CloneBaseData()
        {
            Psm newpsmtoreturn = new Psm
            {
                RawdataFilename = this.RawdataFilename,
                SpectrumNum = this.SpectrumNum,
                RetentionTime = this.RetentionTime,
                PrecurserMassOverCharge = this.PrecurserMassOverCharge,
                PrecurserIntensity = this.PrecurserIntensity,
                MatchingMs2Intensity = this.MatchingMs2Intensity,
                PeptideBaseSequence = this.PeptideBaseSequence,
                ProteinDescription = this.ProteinDescription,
                MissedCleavages = this.MissedCleavages,
                MatchingIntensity = this.MatchingIntensity,
                QValue = this.QValue,
                Target = this.Target,
                ProteinCorrectionFactor = this.ProteinCorrectionFactor,
                ProteinGroupCorrectionFactor = this.ProteinGroupCorrectionFactor
            };

            return newpsmtoreturn;
        }


        public int NumberOfProteinsMatching
        {
            get { return MatchingProteinList.Count; }
        }

        public int NumberOfProteingroupsMatching
        {
            get { return MatchingProteingroupList.Count; }
        }

        
    }
}
