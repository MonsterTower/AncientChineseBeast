using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class CompAbilityEffect_WindBarrier : CompAbilityEffect
    {
        public override void OnGizmoUpdate()
        {
            GenDraw.DrawRadiusRing(parent.pawn.Position, (parent as Ability_WindBarrier).Range - 0.5f);
        }
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return true;
        }
        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return true;
        }
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return true;
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (parent.pawn.def.defName == "SZ_MingShe" || parent.pawn.def.defName == "SZ_MingShe_Friendly")
            {
                parent.StartCooldown(1800);
            }
            (parent as Ability_WindBarrier).ticksTime = 900;
        }
    }
}
