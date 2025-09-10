using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class CompProperties_GenesInCorpse : CompProperties
    {
        public CompProperties_GenesInCorpse()
        {
            compClass = typeof(CompGenesInCorpse);
        }
        public List<GeneDef> genes = new List<GeneDef>();
    }
}
