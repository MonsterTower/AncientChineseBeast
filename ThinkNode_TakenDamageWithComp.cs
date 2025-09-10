using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public class ThinkNode_TakenDamageWithComp : ThinkNode_Priority
    {
        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            if (pawn.GetComp<CompActiveAfterTakenDamage>().actived)
            {
                return base.TryIssueJobPackage(pawn, jobParams);
            }
            return ThinkResult.NoJob;
        }
    }
}
