using Verse;

namespace AncientChineseBeast
{
    public class JobGiver_SeXieKillHuman : JobGiver_KillHuman_RangedAbility
    {
        public override string RangedAbilityDefName(Pawn pawn)
        {
            return pawn.def.defName == "SZ_SeXie" || pawn.def.defName == "SZ_SeXie_Friendly" ? "SZ_SeXieShootA" : "SZ_SeXieShootB";
        }
    }
}
