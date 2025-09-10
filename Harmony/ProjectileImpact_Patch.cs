using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class ProjectileImpact_Patch
    {
        [HarmonyPatch(typeof(Projectile), "CanHit")]
        static class ProjectileImpact_PreFix
        {
            [HarmonyPrefix]
            static bool PreFix(Thing thing, Projectile __instance)
            {
                if (thing.def.defName == "SZ_QiongQi" || thing.def.defName == "SZ_QiongQi_Friendly" || (ModsConfig.BiotechActive && thing is Pawn pawn && pawn.genes?.GetGene(DefDatabase<GeneDef>.GetNamed("SZGene_QiongQi_Eyes")) != null))
                {
                    if (!things.ContainsKey(thing.thingIDNumber))
                    {
                        things.Add(thing.thingIDNumber, new HashSet<KeyValuePair<Thing, bool>>());
                    }
                    foreach (var x in things[thing.thingIDNumber])
                    {
                        if (x.Key == __instance)
                        {
                            return !x.Value;
                        }
                    }
                    if (Rand.Chance(0.5f))
                    {
                        MoteMaker.ThrowText(thing.DrawPos, thing.Map, "TextMote_Dodge".Translate(), 1.9f);
                        things[thing.thingIDNumber].Add(new KeyValuePair<Thing, bool>(__instance, true));
                        return false;
                    }
                    else
                    {
                        things[thing.thingIDNumber].Add(new KeyValuePair<Thing, bool>(__instance, false));
                        return true;
                    }
                }
                return true;
            }
        }
        static Dictionary<int, HashSet<KeyValuePair<Thing, bool>>> things = new Dictionary<int, HashSet<KeyValuePair<Thing, bool>>>();
    }
}
