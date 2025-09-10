using HarmonyLib;
using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class GetBodyPartHealth
    {
        [HarmonyPatch(typeof(BodyPartDef), "GetMaxHealth")]
        static class GetMaxHealth_PostFix
        {
            [HarmonyPostfix]
            static void Postfix(BodyPartDef __instance, Pawn pawn, ref float __result)
            {
                if (ModsConfig.BiotechActive && __instance == BodyPartDefOf.Head && pawn.genes?.GetGene(DefDatabase<GeneDef>.GetNamed("SZGene_YearBeast_Horn")) != null)
                {
                    __result += 50;
                }
            }
        }
    }
}
