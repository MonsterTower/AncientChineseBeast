using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class CompAbilityEffect_BerserkRing : CompAbilityEffect
    {
        public override void OnGizmoUpdate()
        {
            GenDraw.DrawRadiusRing(parent.pawn.Position, 10.9f);
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
            (parent as Ability_BerserkRing).ticksTime = 3601;
        }
    }
}
