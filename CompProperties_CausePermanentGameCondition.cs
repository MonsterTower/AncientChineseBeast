using Verse;

namespace AncientChineseBeast
{
    public class CompProperties_CausePermanentGameCondition : CompProperties
    {
        public CompProperties_CausePermanentGameCondition()
        {
            compClass = typeof(CompCausePermanentGameCondition);
        }
        public GameConditionDef conditionDef;
    }
}
