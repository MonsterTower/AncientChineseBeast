using Verse;

namespace AncientChineseBeast
{
    public class JobGiver_YearKillHuman : JobGiver_KillHuman_RangedAbility
    {
        public override string RangedAbilityDefName(Pawn pawn)
        {
            return "SZ_YearBeast_Flamethrower";
        }
    }
}
