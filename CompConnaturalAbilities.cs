using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class CompConnaturalAbilities : ThingComp
    {
        public CompProperties_ConnaturalAbilities Props => props as CompProperties_ConnaturalAbilities;
        public override void PostPostMake()
        {
            RestoreAbilities();
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            RestoreAbilities();
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent.Faction != null && parent.Faction.IsPlayer)
            {
                foreach (var x in (parent as Pawn).abilities.abilities)
                {
                    foreach (var y in x.GetGizmos())
                    {
                        yield return y;
                    }
                }
            }

        }
        public override void CompTick()
        {
            Pawn pawn = parent as Pawn;
            if (Props.forceHostile && parent.Faction !=Faction.OfPlayer && pawn.IsHashIntervalTick(120))
            {
                if (pawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.ManhunterPermanent && pawn.Awake())
                {
                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, parent.def.LabelCap, false, false, false,null, true, false, false);
                }
            }
            if(parent.Faction == Faction.OfPlayer)
            {
                pawn.mindState.mentalStateHandler.Reset();
            }
        }
        private void RestoreAbilities()
        {
            if (parent is Pawn pawn && pawn.abilities == null)
            {
                pawn.abilities = new Pawn_AbilityTracker(pawn);
                foreach (var x in Props.abilities)
                {
                    pawn.abilities.GainAbility(x);
                }
            }
        }
    }
}
