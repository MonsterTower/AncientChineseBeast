using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AncientChineseBeast
{
    public class CompProperties_AIExpansionFriendly : CompProperties
    {
        public CompProperties_AIExpansionFriendly()
        {
            compClass = typeof(CompAIExpansionFriendly);
        }
        public AbilityDef rangeAbility;
    }
}
