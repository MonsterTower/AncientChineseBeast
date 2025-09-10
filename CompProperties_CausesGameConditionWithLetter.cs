using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class CompProperties_CausesGameConditionWithLetter : CompProperties_CausesGameCondition
    {
        public CompProperties_CausesGameConditionWithLetter()
        {
            compClass = typeof(CompCauseGameCondition_WithLetter);
        }
        [MustTranslate]
        public string text;
    }
}
