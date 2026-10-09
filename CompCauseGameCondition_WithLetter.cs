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
            // This read the private "condition" field off CompCausePermanentGameCondition, a different class
            // in this mod that this comp does not derive from, so FieldInfo.GetValue threw every time and the
            // message below never got posted. The conditions this comp actually causes are the ones its own
            // base class tracks.
            foreach (GameCondition condition in CausedConditions)
            {
                condition.End();
            }
            Messages.Message(Props.text, new TargetInfo(parent.Position, previousMap, false), MessageTypeDefOf.NeutralEvent, true);
        }
    }
}
