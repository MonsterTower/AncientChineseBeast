using RimWorld;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public class CompMingSheAIExpansion : ThingComp
    {
        public override void CompTick()
        {
            if (!parent.Spawned || parent.Faction == Faction.OfPlayer) return;
            if (parent.GetComp<CompActiveAfterTakenDamage>().actived && Find.TickManager.TicksGame % 60 == 0)
            {
                if ((ability ?? (ability = (parent as Pawn).abilities.GetAbility(DefDatabase<AbilityDef>.GetNamed("SZ_MingShe_WindWing")))).CanCast)
                {
                    Pawn pawn = parent as Pawn;
                    pawn.jobs.ClearQueuedJobs();
                    pawn.jobs.TryTakeOrderedJob(ability.GetJob(pawn, pawn), new JobTag?(JobTag.Misc), true);
                }
            }
        }
        private Ability ability;

    }
}
