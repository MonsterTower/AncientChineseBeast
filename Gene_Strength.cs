using Verse;

namespace AncientChineseBeast
{
    public class Gene_Strength : Gene
    {
        public override void Tick()
        {
            if (pawn.equipment.Primary == null)
            {
                if (hediff == null)
                {
                    hediff = pawn.health.AddHediff(HediffDef.Named("SZ_Strength"));
                }
            }
            else
            {
                if (hediff != null && pawn.health.hediffSet.hediffs.Contains(hediff))
                {
                    pawn.health.RemoveHediff(hediff);
                    hediff = null;
                }
            }
        }
        public override void PostRemove()
        {
            if (hediff != null && pawn.health.hediffSet.hediffs.Contains(hediff))
            {
                pawn.health.RemoveHediff(hediff);
                hediff = null;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref hediff, "hediff");
        }
        private Hediff hediff;
    }
}
