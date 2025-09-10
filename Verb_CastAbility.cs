using Verse;

namespace AncientChineseBeast
{
    public class Verb_CastAbility : RimWorld.Verb_CastAbility
    {
        public override void DrawHighlight(LocalTargetInfo target)
        {
            if (!verbProps.targetable)
            {
                this.ability.DrawEffectPreviews(target);
                return;
            }
            base.DrawHighlight(target);
        }
        protected override int ShotsPerBurst
        {
            get
            {
                return verbProps.burstShotCount;
            }
        }
    }
}
