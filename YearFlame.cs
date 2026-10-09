using System;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class YearFlame : Thing, IAnimationThing
    {
        public int Index
        {
            get
            {
                int v = ((fireTime - 3) / 4);
                if (v < 0) return 0;
                if (v <= 8) return v;
                if (v <= 38) return ((v - 3) % 6) + 3;
                return (Math.Min(13, v - 31));
            }
        }
        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            Vector3 drawPos = origin;
            drawPos.y = AltitudeLayer.Projectile.AltitudeFor();
            Graphic.Draw(drawPos, Rot4.North, this, angle);
        }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (origin == default)
            {
                origin = Position.ToVector3Shifted();
            }
        }
        protected override void Tick()
        {
            fireTime++;
            if (fireTime % 12 == 0)
            {
                Attack();
            }
            if (fireTime >= 183)
            {
                Destroy();
            }
        }
        void Attack()
        {
            bool AngleAlgorithm(float angleA, float angleB, float limit)
            {
                return
                    Math.Abs(angleA - angleB) <= (limit * 0.5) ||
                    Math.Abs(angleA - 360f - angleB) <= (limit * 0.5) ||
                    Math.Abs(angleA + 360f - angleB) <= (limit * 0.5);
            }
            foreach (Pawn x in Map.mapPawns.AllPawnsSpawned)
            {
                if (x == launcher)
                {
                    continue;
                }
                Vector3 pos = x.Position.ToVector3Shifted();
                if ((pos - origin).sqrMagnitude < (distance * distance) && AngleAlgorithm((pos - origin).AngleFlat(), angle, angleSet))
                {
                    ((Projectile)GenSpawn.Spawn(ThingDef.Named(launcher.Faction == RimWorld.Faction.OfPlayer ? "SZ_YearBeastFlameProjectile_Small" : "SZ_YearBeastFlameProjectile"), origin.ToIntVec3(), Map, WipeMode.Vanish)).Launch(launcher ?? this, origin, x, x, ProjectileHitFlags.All);
                }
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref fireTime, "fireTime");
            Scribe_Values.Look(ref origin, "origin");
            Scribe_Values.Look(ref angle, "angle");
            Scribe_References.Look(ref launcher, "launcher");
        }
        private int fireTime;
        public float angle = 0;
        public Vector3 origin = default;
        public Thing launcher;

        const float distance = 17f;
        const float angleSet = 40f;
    }
}
