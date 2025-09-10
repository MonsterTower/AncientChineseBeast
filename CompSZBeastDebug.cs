using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class CompSZBeastDebug : ThingComp
    {
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "年兽",
                    action = delegate ()
                    {
                        Singleton.instance.YearBeastForced = true;
                    }
                };
                yield return new Command_Action
                {
                    defaultLabel = "凶兽",
                    action = delegate ()
                    {
                        Singleton.instance.nextBeastTimeHours += 120;
                    }
                };
                yield return new Command_Action
                {
                    defaultLabel = "凶兽Forced",
                    action = delegate ()
                    {
                        Singleton.instance.nextBeastTimeHours += 1200000;
                    }
                };
            }
        }
    }
}
