using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class Graphic_Animation : Graphic_Collection
    {
        public override Material MatSingle
        {
            get
            {
                return subGraphics[0].MatSingle;
            }
        }
        public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
        {
            Graphic graphic;
            if (thing is IAnimationThing animation)
            {
                graphic = subGraphics[animation.Index];
            }
            else
            {
                graphic = subGraphics[0];
            }
            graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
        }
    }
}
