using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class ProteomicsExperimentRun
    {
        public string RawFileName { get; set; }
        public ProteinGroupList ProteingroupList = new ProteinGroupList();
        public PsmList PsmList = new PsmList();
        public string ExperimentId { get; set; }
        public bool Whitelisted { get; set; }
        public bool PsmsMatched { get; set; }
        public bool PsmsSummarized { get; set; }
        public bool OutPutNSAF { get; set; }
        public bool OutputDNSAF { get; set; }
        public bool OutputUNSAF { get; set; }
        public Dictionary<string, string> WhitelistDictionary { get; set; } 
    }
}
