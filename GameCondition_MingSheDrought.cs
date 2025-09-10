using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class GameCondition_MingSheDrought : GameCondition
    {
        public override WeatherDef ForcedWeather()
        {
            return WeatherDefOf.Clear;
        }
        public override void GameConditionTick()
        {
            if (conditionCauser == null || !conditionCauser.Spawned || (conditionCauser as Pawn).Dead || conditionCauser.Faction == Faction.OfPlayer)
            {
                End();
                return;
            }
            if (Find.TickManager.TicksGame % 2500 == 0)
            {
                things.AddRange(SingleMap.listerThings.ThingsInGroup(ThingRequestGroup.Plant));
                foreach (var x in things)
                {
                    if (x.Map == conditionCauser.Map && !CheckPlant(x.def.defName))
                    {
                        x.TakeDamage(new DamageInfo(DamageDefOf.Rotting, Rand.Range(7, 20)));
                    }
                }
                things.Clear();
            }
        }
        private static List<Thing> things = new List<Thing>();
        public static bool CheckPlant(string defName)
        {
            return defName == "Plant_GrassAnima" ||
                defName == "Plant_TreeAnima" ||
                defName == "Plant_MossGauranlen" ||
                defName == "Plant_TreeGauranlen" ||
                defName == "Plant_TreePolux";
        }
    }
}
