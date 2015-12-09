using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morpheus_Spectral_Counter
{
    [Serializable]
    public class ProteinGroup : IQValue
    {
        //Properties
       
        public string ProteingroupId { get; set; }
        public double QValue { get; set; }
        public string WhiteListName { get; set; }
        public int UniquePsms { get; set; }
        public int SharedPsms { get; set; }
        public double CorrectedPsms { get; set; }
        public double NormalizedSpectralAbundanceFactor { get; set; }
        public double NormalizedCorrectedSpectralAbundanceFactor { get; set; }
        public double NormalizedUniqueSpectralAbundanceFactor { get; set; }
        public double PrecursurIntensity { get; set; }
        public double MatchingMs2Intensity { get; set; }

        public double NumberOfUniquePeptides { get { return UniquePeptideHashSet.Count(); } }

        public double AverageSequenceCoverage { get { return CalculateProteinSequenceCoverage(); } }
        public double CorrectedSpectralAbundanceFactor { get { return (double)TotalCorrectedPsms / AverageLength; } }
        public double SpectralAbundanceFactor { get { return (double)TotalPsms / AverageLength; } }
        //This needs to be modified with unique length as per washburn's paper that describes, only dividing by unique length improves performance overall
        public double UniqueSpectralAbundanceFactor { get { return (double) UniquePsms/AverageLength; } }
        public double AverageLength { get { return CalculateAverageProteinLength(); } }

        
        public double TotalPsms { get { return CalculateTotalPsms(); } }
        public int ProteinCount{get { return ProteingroupList.Count; } }
        public double TotalCorrectedPsms { get { return CalculateTotalCorrectedPsms(); } }

        public List<Protein> ProteingroupList = new List<Protein>();
        public List<ProteinGroup> SharedGroupList = new List<ProteinGroup>();
        public HashSet<String> UniquePeptideHashSet = new HashSet<string>(); 
        //Unused but could be adapted to calculate more refined forms of NSAF,uNSAF, and dNSAF
        /*
        public double SharedDetectedLength { get; set; }
        public double UniqueDetectedLength { get; set; }
        public double IdentifiedResidueLength { get { return AverageSequenceCoverage * AverageLength; } }
        public List<String> DetectedUniquePeptides = new List<String>();
        public List<String> SharedUniquePeptides = new List<String>();
         */
        
        //Methods
        public ProteinGroup CloneBaseData()
        {
            ProteinGroup proteinGroupToReturn = new ProteinGroup()
            {
                ProteingroupId = ProteingroupId,
                QValue = QValue,
                WhiteListName = WhiteListName,
                UniquePsms = UniquePsms,
                SharedPsms = SharedPsms,
                CorrectedPsms = CorrectedPsms,
                NormalizedSpectralAbundanceFactor = NormalizedSpectralAbundanceFactor,
                NormalizedCorrectedSpectralAbundanceFactor = NormalizedCorrectedSpectralAbundanceFactor,
                NormalizedUniqueSpectralAbundanceFactor = NormalizedUniqueSpectralAbundanceFactor
            };
            foreach(Protein p in ProteingroupList)
            {
                Protein newProteinToAdd = new Protein();

                newProteinToAdd = p.CloneBaseData();

                proteinGroupToReturn.ProteingroupList.Add(newProteinToAdd);
            }

            foreach(ProteinGroup pg in SharedGroupList)
            {
                ProteinGroup proteinGroupToAdd = new ProteinGroup();
                proteinGroupToAdd = pg.CloneBaseData();
                proteinGroupToReturn.SharedGroupList.Add(proteinGroupToAdd);
            }

            return proteinGroupToReturn;
        }

       
        public double CalculateProteinSequenceCoverage()
        {
            double averageProteinSequenceCoverage = 0;
            foreach (Protein p in ProteingroupList)
            {
                averageProteinSequenceCoverage += p.SequenceCoverage;
            }
            averageProteinSequenceCoverage = averageProteinSequenceCoverage/ProteingroupList.Count;
            return averageProteinSequenceCoverage;
        }

        public double CalculateAverageProteinLength()
        {
            int summedProteinLength = 0;
            double proteinLengthAverage = 0;
            foreach (Protein p in ProteingroupList)
            {
                summedProteinLength += p.Sequence.Length;
            }
            proteinLengthAverage = (double) summedProteinLength/ProteingroupList.Count;
            return proteinLengthAverage;
        }



        public double CalculateTotalCorrectedPsms()
        {
            double totalCorrectedPsms = 0;
            if (SharedGroupList.Count == 0)
            {
                totalCorrectedPsms = UniquePsms;
            }
            else
            {
                totalCorrectedPsms = UniquePsms + CorrectedPsms;
            }
            return totalCorrectedPsms;
        }

        public double CalculateTotalPsms()
        {
            return this.SharedPsms + this.UniquePsms;
        }
    }
}
