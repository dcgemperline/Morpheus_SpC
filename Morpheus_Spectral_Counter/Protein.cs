﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    [Serializable]
    public class Protein
    {
        public string ProteinId { get; set; }
        public string Sequence { get; set; }
        public double SequenceCoverage { get; set; }
    }
}
