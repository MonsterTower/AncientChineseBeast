using HarmonyLib;
using JetBrains.Annotations;
using System.Reflection;
using Verse;

namespace AncientChineseBeast
{
    [UsedImplicitly]
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        static PatchMain()
        {
            var instance = new Harmony("SZBeast_HarmonyPatches");
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
