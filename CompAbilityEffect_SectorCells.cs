using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class CompAbilityEffect_SectorCells : CompAbilityEffect
    {
        public static IntVec3 casterCache;
        public static float angleCache;
        public static float radiusCache;
        public static Vector3 targetCache;
        public static List<IntVec3> resultCache = new List<IntVec3>();
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            GenDraw.DrawFieldEdges(GetSectorCells(target.CenterVector3, parent.verb.verbProps.AdjustedRange(parent.verb, parent.pawn), 40, (IntVec3 c) => GenSight.LineOfSight(parent.pawn.Position, c, parent.pawn.Map)));
        }
        public List<IntVec3> GetSectorCells(Vector3 target, float radius, float angle, Func<IntVec3, bool> predicate = null)
        {

            if (parent.pawn.Position == casterCache && target == targetCache && angleCache == angle && radiusCache == radius)
            {
                return resultCache;
            }
            List<IntVec3> cells = new List<IntVec3>();
            List<IntVec3> inCells = new List<IntVec3>();
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = parent.pawn.Position + GenRadial.RadialPattern[i];
                cells.Add(intVec);
            }
            float targetAngle = (target - parent.pawn.TrueCenter()).AngleFlat();
            foreach (IntVec3 x in cells)
            {
                if (AngleAlgorithm((x.ToVector3Shifted() - parent.pawn.TrueCenter()).AngleFlat(), targetAngle, angle) && (predicate == null || predicate(x)))
                {
                    inCells.Add(x);
                }
            }
            casterCache = parent.pawn.Position;
            targetCache = target;
            angleCache = angle;
            resultCache.Clear();
            resultCache.AddRange(resultCache);
            return inCells;

            bool AngleAlgorithm(float angleA, float angleB, float limit)
            {
                return
                    Math.Abs(angleA - angleB) <= (limit * 0.5) ||
                    Math.Abs(angleA - 360f - angleB) <= (limit * 0.5) ||
                    Math.Abs(angleA + 360f - angleB) <= (limit * 0.5);
            }
        }
    }
}