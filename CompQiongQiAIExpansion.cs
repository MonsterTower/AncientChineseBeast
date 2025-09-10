using RimWorld;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public class CompQiongQiAIExpansion : ThingComp
    {
        public override void CompTick()
        {
            if (!parent.Spawned || parent.Faction == Faction.OfPlayer) return;
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                if ((ability ?? (ability = (parent as Pawn).abilities.GetAbility(DefDatabase<AbilityDef>.GetNamed("SZ_QiongQi_FlyingStrike")))).CanCast)
                {
                    Pawn target = null;
                    float distanceS = -1;
                    foreach (var x in parent.Map.mapPawns.AllPawns)
                    {
                        if (!x.Downed && ((x.Faction != null && x.Faction.IsPlayer) || x.RaceProps.Humanlike))
                        {
                            if (ability.VerbTracker.PrimaryVerb.CanHitTarget(x))
                            {
                                float dx = parent.Position.x - x.Position.x;
                                float dz = parent.Position.z - x.Position.z;
                                float distance = (dx * dx) + (dz * dz);
                                if (distance > distanceS)
                                {
                                    distanceS = distance;
                                    target = x;
                                }
                            }
                        }
                    }
                    if (target != null)
                    {
                        Pawn pawn = parent as Pawn;
                        pawn.jobs.ClearQueuedJobs();
                        pawn.jobs.TryTakeOrderedJob(ability.GetJob(target, target.Position), new JobTag?(JobTag.Misc), false);
                    }
                }
            }
        }
        private Ability ability;

    }
}
