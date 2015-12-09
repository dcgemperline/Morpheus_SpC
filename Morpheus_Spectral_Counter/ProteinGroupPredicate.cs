using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Morpheus_Spectral_Counter
{
    public class ProteinGroupPredicate
    {
        public ProteinGroupPredicate(int minimnumberofUniquePeptides)
        {
            MinimumNumberofUniquePeptides = minimnumberofUniquePeptides;
        }


        private double MinimumNumberofUniquePeptides;
        
        public Predicate<ProteinGroup> BuildProteinGroupPredicate()
        {
            Predicate<ProteinGroup> predicate = FilterProteinGroupConditions;

            return predicate;

        }

        bool FilterProteinGroupConditions(ProteinGroup obj)
        {
            return obj.NumberOfUniquePeptides >= MinimumNumberofUniquePeptides;
        }

    }

}
