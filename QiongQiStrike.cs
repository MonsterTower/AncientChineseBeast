using UnityEngine;
using Verse;

namespace AncientChineseBeast
{

    public class QiongQiStrike : Thing, IAnimationThing
    {
        public override Graphic Graphic
        {
            get
            {
                if (isLarge)
                {
                    if (largeGraphicInt == null)
                    {
                        GraphicData graphicData = new GraphicData();
                        graphicData.CopyFrom(def.graphicData);
                        graphicData.drawSize = new Vector2(9.5f, 9.5f);
                        largeGraphicInt = graphicData.Graphic;
                    }
                    return largeGraphicInt;
                }
                return base.Graphic;
            }
        }
        public int Index
        {
            get
            {
                return spawnTime / 4;
            }
        }
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {

            Graphic.Draw(drawLoc, Rot4.North, this, angle);
        }
        protected override void Tick()
        {
            spawnTime++;
            if (spawnTime >= 20)
            {
                Destroy();
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref spawnTime, "spawnTime");
            Scribe_Values.Look(ref angle, "angle");
            Scribe_Values.Look(ref isLarge, "isLarge");
        }
        private int spawnTime;
        public bool isLarge;
        public float angle;
        private Graphic largeGraphicInt;
    }
}
