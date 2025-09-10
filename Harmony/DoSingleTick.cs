using HarmonyLib;
using System;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class DoSingleTick
    {
        [HarmonyPatch(typeof(TickManager), "DoSingleTick", new Type[] { })]
        static class DoSingleTick_PreFix
        {
            [HarmonyPrefix]
            static bool PreFix()
            {
                if (Singleton.instance == null)
                {
                    Singleton.instance = new Singleton();
                }
                Singleton.instance.Tick();
                return true;
            }
        }
    }
}
