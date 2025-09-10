using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class DefModExtension_GeneHediffs : DefModExtension
    {
        public List<HediffDefWithBodyPartDef> hediffs;
    }
    public class HediffDefWithBodyPartDef
    {
        public BodyPartDef bodyPart;
        public BodyPartGroupDef bodyPartGroup;
        public HediffDef hediff;
    }
}
