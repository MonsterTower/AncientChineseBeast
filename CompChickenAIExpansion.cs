using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class CompChickenAIExpansion : ThingComp
    {
        public override void CompTick()
        {
            if (parent == null || !parent.Spawned)
            {
                return;
            }
            if (Find.TickManager.TicksGame - time > 30000 && GenLocalDate.HourOfDay(parent.Map) == 4)
            {
                time = Find.TickManager.TicksGame;
                if ((ability ?? (ability = (parent as Pawn).abilities.GetAbility(DefDatabase<AbilityDef>.GetNamed("SZ_Chicken_Crow")))).CanCast)
                {
                    ability.Activate(parent, parent);
                }
            }
        }
        private int time;
        private Ability ability;
    }
}
