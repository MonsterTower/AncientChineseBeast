using RimWorld;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public abstract class JobGiver_KillHuman_RangedAbility : JobGiver_KillHuman
    {
        public abstract string RangedAbilityDefName(Pawn pawn);
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.TryGetAttackVerb(null, false, false) == null)
            {
                return null;
            }
            if (pawn.mindState.enemyTarget == null || !pawn.mindState.enemyTarget.Spawned || (pawn.mindState.enemyTarget is Pawn pawn2 && pawn2.Downed))
            {
                pawn.mindState.enemyTarget = FindPawnTarget(pawn);
                if (pawn.mindState.enemyTarget == null)
                {
                    return null;
                }
            }
            if (pawn.abilities.GetAbility(DefDatabase<AbilityDef>.GetNamed(RangedAbilityDefName(pawn))) is Ability ability && pawn.Position.Standable(pawn.Map))
            {
                if (ability.verb.CanHitTarget(pawn.mindState.enemyTarget))
                {
                    return ability.GetJob(pawn.mindState.enemyTarget, pawn.mindState.enemyTarget);
                }
                if (TryFindShootingPosition(pawn, pawn.mindState.enemyTarget, out IntVec3 goodPos, ability.verb))
                {
                    Job job = JobMaker.MakeJob(JobDefOf.Goto, goodPos);
                    job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
                    job.checkOverrideOnExpire = true;
                    return job;
                }
            }
            IntVec3 intVec = pawn.mindState.enemyTarget.PositionHeld;
            if (pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.PassAllDestroyableThings))
            {
                using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false), PathEndMode.OnCell, null))
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
        protected bool TryFindShootingPosition(Pawn pawn, Thing target, out IntVec3 dest, Verb verbToUse = null)
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
