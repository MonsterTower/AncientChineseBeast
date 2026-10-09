using RimWorld;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public class Verb_FlyingStrike : Verb
    {
        public override bool MultiSelect
        {
            get
            {
                return true;
            }
        }
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            return caster != null && CanHitTarget(target) && JumpUtility.ValidJumpTarget(caster, caster.Map, target.Cell);
        }
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            float num = EffectiveRange * EffectiveRange;
            IntVec3 cell = targ.Cell;
            return caster.Position.DistanceToSquared(cell) <= num && GenSight.LineOfSight(root, cell, this.caster.Map, false, null, 0, 0);
        }
        public override void DrawHighlight(LocalTargetInfo target)
        {
            base.DrawHighlight(target);
            GenDraw.DrawLineBetween(caster.TrueCenter(), target.CenterVector3);
        }
        public override void OnGUI(LocalTargetInfo target)
        {
            if (CanHitTarget(target) && JumpUtility.ValidJumpTarget(caster, this.caster.Map, target.Cell))
            {
                base.OnGUI(target);
                return;
            }
            GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
        }
        public override void OrderForceTarget(LocalTargetInfo target)
        {
            Job job = JobMaker.MakeJob(JobDefOf.CastJump, target);
            job.verbToUse = this;
            CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }
        protected override bool TryCastShot()
        {
            if (!(DirectOwner as Ability).CanCast)
            {
                CasterPawn.jobs.StopAll();
                return false;
            }
            Pawn casterPawn = CasterPawn;
            IntVec3 cell = currentTarget.Cell;
            IntVec3 start = casterPawn.Position;
            Map map = casterPawn.Map;
            (DirectOwner as Ability).Activate(currentTarget, currentTarget.Cell);
            QiongQiStrike strike = ThingMaker.MakeThing(ThingDef.Named("SZ_QQAnimation")) as QiongQiStrike;
            strike.angle = (cell.ToVector3Shifted() - start.ToVector3Shifted()).AngleFlat();
            if (CasterPawn.def.defName == "SZ_QiongQi" || CasterPawn.def.defName == "SZ_QiongQi_Friendly")
            {
                strike.isLarge = true;
            }
            GenSpawn.Spawn(strike, start, map, WipeMode.Vanish);

            PawnFlyingStrike pawnStrike = PawnFlyingStrike.Make(ThingDef.Named("SZ_QQPawnFlyingStrike"), DirectOwner as Ability, CasterPawn, currentTarget.Cell, null, null);
            if (pawnStrike != null)
            {
                GenSpawn.Spawn(pawnStrike, cell, map, WipeMode.Vanish);
                return true;
            }
            return false;
        }
    }
}
