using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class Ability_WindBarrier : Ability_Draw
    {
        public Ability_WindBarrier(Pawn pawn) : base(pawn)
        {
        }
        public Ability_WindBarrier(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
        }
        public float Range => pawn.def.defName == "SZ_MingShe" || pawn.def.defName == "SZ_MingShe_Friendly" ? 10f : 5f;
        public override void AbilityTick()
        {
            bool CollisionDetermination(Vector3 a, Vector3 b, float range)
            {
                float dx = (a.x - b.x);
                float dy = (a.z - b.z);
                return (dx * dx) + (dy * dy) <= range * range;
            }

            base.AbilityTick();
            if (ticksTime > 0)
            {
                ticksTime--;
                if (ticksTime % 30 == 0)
                {
                    int num = GenRadial.NumCellsInRadius(Range - 0.5f);
                    for (int i = 0; i < num; i++)
                    {
                        IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[i];
                        if (intVec.InBounds(pawn.Map))
                        {
                            foreach (Thing x in intVec.GetThingList(pawn.Map))
                            {
                                if ((pawn.HostileTo(x) || pawn.def.defName == "SZ_MingShe") && pawn != x)
                                {
                                    targets.Add(x);
                                }
                            }
                        }
                    }
                    while (targets.Count > 0)
                    {
                        Thing target = targets[targets.Count - 1];
                        target.TakeDamage(new DamageInfo(DamageDefOf.Cut, 10, 100, -1, pawn));
                        targets.RemoveAt(targets.Count - 1);
                    }
                }
                foreach (Projectile projectile in StoreProjectiles.projectiles.Where(projectile => projectile.Spawned && !projectile.Destroyed))
                {
                    if (projectile.Map == pawn.Map && projectile.Launcher != pawn && CollisionDetermination(projectile.ExactPosition, pawn.TrueCenter(), Math.Max(Range, Range * projectile.def.projectile.SpeedTilesPerTick)))
                    {
                        projectiles.Add(projectile);
                    }
                }
                while (projectiles.Count > 0)
                {
                    if (projectiles[0].Launcher != null && !(projectiles[0].Launcher is Pawn target && pawn.Downed) && projectiles[0].Launcher.Spawned)
                    {
                        Vector3 origin = (Vector3)typeof(Projectile).GetField("origin", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(projectiles[0]);

                        if (!CollisionDetermination(origin, pawn.TrueCenter(), Range))
                        {
                            LocalTargetInfo localTarget = new LocalTargetInfo(projectiles[0].Launcher);
                            if (Rand.Chance(0.5f))
                            {
                                localTarget = new LocalTargetInfo(localTarget.Cell + Cells.EightCells.RandomElement());
                            }
                            ThingDef projectile = projectiles[0].def;
                            Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, projectiles[0].DrawPos.ToIntVec3(), pawn.Map, WipeMode.Vanish);
                            projectile2.Launch(pawn, projectiles[0].DrawPos, localTarget, projectiles[0].Launcher, ProjectileHitFlags.All, false, ThingMaker.MakeThing(projectiles[0].EquipmentDef), null);
                        }
                    }
                    // A projectile can already be gone by now (it hit something in the same tick, or it is in the list twice):
                    // destroying it again threw a null reference out of ListerThings.Remove and the whole tick of the mingshe with it.
                    if (projectiles[0].Spawned && !projectiles[0].Destroyed)
                    {
                        projectiles[0].Destroy();
                    }
                    projectiles.RemoveAt(0);
                }
            }
        }

        public override void Draw(Vector3 drawLoc)
        {
            if (ticksTime < 1)
            {
                return;
            }
            if (ticksTime <= 60)
            {
                float alpha = ticksTime / 60f;
                Color color = new Color(1, 1, 1, alpha);
                WindA.color = color;
                WindB.color = color;
                WindC.color = color;
                Shield.color = color;
            }
            else
            {
                WindA.color = Color.white;
                WindB.color = Color.white;
                WindC.color = Color.white;
                Shield.color = Color.white;
            }
            Vector3 s = new Vector3(Range * 2, 1f, Range * 2);
            int tick = Find.TickManager.TicksGame % 360;
            Matrix4x4 matrix = default;
            matrix.SetTRS(drawLoc, Quaternion.AngleAxis(tick * 7f, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, WindA, 0);
            Matrix4x4 matrixB = default;
            matrixB.SetTRS(drawLoc, Quaternion.AngleAxis(tick * 11f, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrixB, WindB, 0);
            Matrix4x4 matrixC = default;
            matrixC.SetTRS(drawLoc, Quaternion.AngleAxis(tick * 13f, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrixC, WindC, 0);
            Matrix4x4 matrixD = default;
            matrixD.SetTRS(drawLoc, Quaternion.AngleAxis(0, Vector3.up), new Vector3(Range * 2.1f, 1f, Range * 2.1f));
            Graphics.DrawMesh(MeshPool.plane10, matrixD, Shield, 0);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksTime, "ticksTime");
        }
        public int ticksTime;
        static List<Projectile> projectiles = new List<Projectile>();
        static List<Thing> targets = new List<Thing>();
        private static readonly Material WindA = MaterialPool.MatFrom("AncientChineseBeast/MingShe/WindA", ShaderDatabase.Transparent);
        private static readonly Material WindB = MaterialPool.MatFrom("AncientChineseBeast/MingShe/WindB", ShaderDatabase.Transparent);
        private static readonly Material WindC = MaterialPool.MatFrom("AncientChineseBeast/MingShe/WindC", ShaderDatabase.Transparent);
        public static readonly Material Shield = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);
    }
}
