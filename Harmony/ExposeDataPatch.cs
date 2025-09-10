using HarmonyLib;
using System;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class ExposeDataPatch
    {
        [HarmonyPatch(typeof(Game), "ExposeSmallComponents", new Type[] { })]
        static class ExposeDataPatch_PostFix
        {
            [HarmonyPostfix]
            static void PostFix()
            {
                if (Singleton.instance == null)
                {
                    Singleton.instance = new Singleton();
                }
                Scribe_Deep.Look(ref Singleton.instance, "SZBeast_Singleton");
            }
        }
    }
}
