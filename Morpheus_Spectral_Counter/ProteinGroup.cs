using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    [Serializable]
    public class ProteinGroup : IQValue, ICloneable
    {
        //Properties
        public List<Protein> ProteingroupList = new List<Protein>();
        public List<ProteinGroup> SharedGroupList = new List<ProteinGroup>();
        public string ProteingroupId { get; set; }
        public double QValue { get; set; }
        public string WhiteListName { get; set; }
        public double AverageSequenceCoverage { get { return CalculateProteinSequenceCoverage(); } }
        public double CorrectedSpectralAbundanceFactor { get { return (double)TotalCorrectedPsms / AverageLength; } }
        public double NormalizedCorrectedSpectralAbundanceFactor { get; set; }
        public double SpectralAbundanceFactor { get { return (double)TotalPsms / AverageLength; } }
        public double NormalizedSpectralAbundanceFactor { get; set; }
        //This needs to be modified with unique length as per washburn's paper that describes, only divgiding by unique length improves performance overall
        public double UniqueSpectralAbundanceFactor { get { return (double) UniquePsms/AverageLength; } }
        public double NormalizedUniqueSpectralAbundanceFactor { get; set; }
        public double AverageLength { get { return CalculateAverageProteinLength(); } }
        public int UniquePsms { get; set; }
        public int SharedPsms { get; set; }
        public double CorrectedPsms { get; set; }
        public double TotalPsms { get { return CalculateTotalPsms(); } }
        public int ProteinCount
        {
            get { return ProteingroupList.Count; }
        }
        public double TotalCorrectedPsms { 
            get { return CalculateTotalCorrectedPsms(); }
        }


        //Methods
        public object Clone()
        {
            return this.MemberwiseClone();
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
