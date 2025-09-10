using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class DoRecipe
    {
        [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts", new Type[] { typeof(RecipeDef), typeof(Pawn), typeof(List<Thing>), typeof(Thing), typeof(IBillGiver), typeof(Precept_ThingStyle), typeof(ThingStyleDef), typeof(int?) })]
        static class DoRecipe_PreFix
        {
            [HarmonyPostfix]
            static void Post(RecipeDef recipeDef, ref IEnumerable<Thing> __result)
            {
                if (ModsConfig.BiotechActive && recipeDef.GetModExtension<DefModExtension_Genes>() is DefModExtension_Genes x)
                {
                    Genepack genepack = (Genepack)ThingMaker.MakeThing(ThingDefOf.Genepack, null);
                    List<GeneDef> genesToAdd = new List<GeneDef>() { x.gene };
                    genepack.Initialize(genesToAdd);
                    __result = new List<Thing>() { genepack };
                }
                if (recipeDef.GetModExtension<DefModExtension_CloneBeast>() is DefModExtension_CloneBeast y)
                {
                    __result = new List<Thing>() { PawnGenerator.GeneratePawn(y.pawn,Faction.OfPlayer) };
                }
            }
        }
    }
}
