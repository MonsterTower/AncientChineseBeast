using HarmonyLib;
using System;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class StoreProjectiles
    {
        [HarmonyPatch(typeof(ThingWithComps), "SpawnSetup", new Type[] { typeof(Map), typeof(bool) })]
        static class ProjectilesSpawn_PostFix
        {
            [HarmonyPostfix]
            static void Postfix(ThingWithComps __instance)
            {
                if (__instance is Projectile projectile)
                {
                    projectiles.Add(projectile);
                }
            }
        }
        [HarmonyPatch(typeof(ThingWithComps), "DeSpawn", new Type[] { typeof(DestroyMode) })]
        static class ProjectilesDeSpawn_PostFix
        {
            [HarmonyPostfix]
            static void Postfix(ThingWithComps __instance)
            {
                if (__instance is Projectile projectile)
                {
                    projectiles.Remove(projectile);
                }
            }
        }
        public static List<Projectile> projectiles = new List<Projectile>();
    }
}
