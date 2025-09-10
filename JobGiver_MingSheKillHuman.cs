using Verse;

namespace AncientChineseBeast
{
    public class JobGiver_MingSheKillHuman : JobGiver_KillHuman_RangedAbility
    {
        public override string RangedAbilityDefName(Pawn pawn)
        {
            return "SZ_MingShe_SoundWave";
        }
    }
}
