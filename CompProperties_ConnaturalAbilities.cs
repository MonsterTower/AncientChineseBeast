using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class CompProperties_ConnaturalAbilities : CompProperties
    {
        public CompProperties_ConnaturalAbilities()
        {
            compClass = typeof(CompConnaturalAbilities);
        }
        public List<AbilityDef> abilities = new List<AbilityDef>();
        public bool forceHostile = true;
    }
}
