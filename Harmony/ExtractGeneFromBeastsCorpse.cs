using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class ExtractGeneFromBeastsCorpse
    {
        [HarmonyPatch(typeof(Building_GeneExtractor), "GetFloatMenuOptions")]
        static class GetFloatMenuOptions_PostFix
        {
            [HarmonyPostfix]
            private static void PostFix(Building_GeneExtractor __instance, Pawn selPawn, ref IEnumerable<FloatMenuOption> __result)
            {
                if(ResearchProjectDef.Named("SZ_BeastGene").IsFinished && __instance.PowerOn)
                foreach (var x in selPawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
                {
                    if(x is Corpse corpse && corpse.InnerPawn.GetComps<CompGenesInCorpse>() != null)
                    {
                            if (!selPawn.CanReach(__instance, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
                            {
                                __result = __result.AddItem(new FloatMenuOption("SZ_CannotReachBuildingToExtractGene".Translate(), null));
                                return;
                            }
                            if (selPawn.CanReach(x, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
                            {
                                __result = __result.AddItem(new FloatMenuOption(x.Label, delegate ()
                                {
                                    selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("SZ_ExtractGeneFromBeastCorpse"), __instance, x), new JobTag?(JobTag.Misc), false);
                                }));
                            }
                    }
                }
            }
        }
    }
}
