using RimWorld;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public class JobGiver_QiongQiKillHuman : JobGiver_KillHuman
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.mindState.enemyTarget == null || !pawn.mindState.enemyTarget.Spawned || (pawn.mindState.enemyTarget is Pawn pawn2 && pawn2.Downed))
            {
                pawn.mindState.enemyTarget = FindPawnTarget(pawn);
                if (pawn.mindState.enemyTarget == null)
                {
                    return null;
                }
            }
            if (pawn.mindState.enemyTarget != null && pawn.CanReach(pawn.mindState.enemyTarget, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
            {
                Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn.mindState.enemyTarget);
                job.expiryInterval = Rand.Range(420, 900);
                job.attackDoorIfTargetLost = true;
                job.canBashFences = true;
                return job;
            }
            IntVec3 intVec = pawn.mindState.enemyTarget.PositionHeld;
            if (pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings))
            {
                using (PawnPath pawnPath = pawn.Map.pathFinder.FindPathNow(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false), null, PathEndMode.OnCell))
                {
                    Thing thing = pawnPath.FirstBlockingBuilding(out IntVec3 cellBeforeBlocker, pawn);
                    if (thing != null)
                    {
                        Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
                        job.ignoreDesignations = true;
                        job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
                        job.checkOverrideOnExpire = true;
                        return job;
                    }
                }
            }
            return null;
        }
        protected bool TryFindShootingPosition(Pawn pawn, Pawn target, out IntVec3 dest, Verb verbToUse = null)
        {
            return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
            {
                caster = pawn,
                target = target,
                verb = verbToUse,
                maxRangeFromTarget = verbToUse.verbProps.range,
                wantCoverFromTarget = (verbToUse.verbProps.range > 5f)
            }, out dest);
        }
    }
}
