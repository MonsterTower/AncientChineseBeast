using HarmonyLib;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class AbilityDrawer
    {
        [HarmonyPatch(typeof(Pawn), "DrawAt")]
        static class AbilityDrawer_PostFix
        {
            [HarmonyPostfix]
            static void PostFix(Pawn __instance, Vector3 drawLoc)
            {
                if (__instance.abilities != null)
                {
                    foreach (var x in __instance.abilities.abilities)
                    {
                        if (x is Ability_Draw ability)
                        {
                            ability.Draw(drawLoc);
                        }
                    }
                }
            }
        }
    }
}
