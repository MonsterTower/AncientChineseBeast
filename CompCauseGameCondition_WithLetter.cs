using RimWorld;
using System.Reflection;
using Verse;

namespace AncientChineseBeast
{
    public class CompCauseGameCondition_WithLetter : CompCauseGameCondition
    {
        public new CompProperties_CausesGameConditionWithLetter Props
        {
            get
            {
                return (CompProperties_CausesGameConditionWithLetter)props;
            }
        }
        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            (typeof(CompCausePermanentGameCondition).GetField("condition", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this) as GameCondition).End();
            Messages.Message(Props.text, new TargetInfo(parent.Position, previousMap, false), MessageTypeDefOf.NeutralEvent, true);
        }
    }
}
