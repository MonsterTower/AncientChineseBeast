using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    namespace AncientChineseBeast
    {
        [StaticConstructorOnStartup]
        public class DraftController
        {
            [HarmonyPatch(typeof(Pawn_DraftController), "ShowDraftGizmo",MethodType.Getter)]
            static class Pawn_DraftController_Post
            {
                [HarmonyPostfix]
                static void PostFix(Pawn_DraftController __instance,ref bool __result)
                {
                    if(__instance.pawn == pawn)
                    {
                        __result = true;
                    }
                    if (__instance.pawn.TryGetComp<CompConnaturalAbilities>() !=null && __instance.pawn.Faction == Faction.OfPlayer)
                    {
                        pawn = __instance.pawn;
                        __result = true;
                    }
                }
            }
            [HarmonyPatch(typeof(Pawn), "IsColonistPlayerControlled", MethodType.Getter)]
            static class Pawn_IsColonistPlayerControlled_Post
            {
                [HarmonyPostfix]
                static void PostFix(Pawn __instance, ref bool __result)
                {
                    if (__instance == pawn)
                    {
                        __result = true;
                    }
                    if (__instance.TryGetComp<CompConnaturalAbilities>() != null && __instance.Faction == Faction.OfPlayer)
                    {
                        pawn = __instance;
                        __result = true;
                    }
                }
            }
            static Pawn pawn;
        }
    }
}
