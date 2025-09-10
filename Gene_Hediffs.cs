using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class Gene_Hediffs : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            foreach (var x in def.GetModExtension<DefModExtension_GeneHediffs>().hediffs)
            {
                List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
                foreach (var y in bpList)
                {
                    if (x.bodyPart == y.def || y.groups.Contains(x.bodyPartGroup))
                    {
                        pawn.health.AddHediff(x.hediff, y);
                    }
                }
            }
        }
        public override void PostRemove()
        {
            base.PostRemove();
            foreach (var x in def.GetModExtension<DefModExtension_GeneHediffs>().hediffs)
            {
                Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(x.hediff, false);
                pawn.health.RemoveHediff(firstHediffOfDef);
            }
        }
    }
}
