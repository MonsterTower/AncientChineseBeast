using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    [StaticConstructorOnStartup]
    public class CompSeXieExpansion : ThingComp
    {
        public override void CompTick()
        {
            if (!parent.Spawned || parent.Faction == Faction.OfPlayer) return;
            if ((parent as Pawn).Downed)
            {
                if (parent.def.defName == "SZ_SeXie")
                {
                    parent.Kill();
                }
                return;
            }
            BerserkRing();
            BerserkPerMintutes();
        }
        private void BerserkRing()
        {
            if (parent.def.defName == "SZ_SeXie")
            {
                return;
            }
            if (human != null)
            {
                if (human.Spawned)
                {
                    human.Destroy();
                }
                if (human.Corpse != null && human.Corpse.Spawned)
                {
                    human.Corpse.Destroy();
                }
            }
            if (ticks % 1800 == 0)
            {
                pawnsLocal.Clear();
                pawnsLocal.AddRange(parent.Map.mapPawns.AllPawnsSpawned.Where((Pawn x) => x.RaceProps.Humanlike));
            }
            foreach (var x in pawnsLocal)
            {
                if (x.Spawned && !x.Downed && x.Position.InHorDistOf(parent.Position, 10.9f))
                {
                    float v = x.GetStatValue(StatDefOf.PsychicSensitivity);
                    if (v > 0 && x.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true))
                    {

                        x.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = (int)(3600 * v);
                    }
                }
            }
        }
        public override void PostDraw()
        {
            if (parent.def.defName == "SZ_SeXie" || !parent.Spawned)
            {
                return;
            }
            Matrix4x4 matrixD = default;
            matrixD.SetTRS(parent.DrawPos, Quaternion.AngleAxis(0, Vector3.up), new Vector3(10.9f * 2.1f, 1f, 10.9f * 2.1f));
            Graphics.DrawMesh(MeshPool.plane10, matrixD, Ring, 0);
        }
        private void BerserkPerMintutes()
        {
            ticks++;
            if (ticks > 3600)
            {
                float max = 0f;
                ticks = 0;
                foreach (var x in parent.Map.mapPawns.FreeColonistsAndPrisonersSpawned)
                {
                    if (x.mindState.Active && x.mindState.mentalStateHandler.InMentalState)
                    {
                        continue;
                    }
                    float value = x.GetStatValue(StatDefOf.PsychicSensitivity);
                    if (value > max)
                    {
                        pawns.Clear();
                        max = value;
                    }
                    if (value == max)
                    {
                        pawns.Add(x);
                    }
                }
                if (max > 0 && pawns.Any())
                {
                    Pawn target = pawns.RandomElement();
                    if (target.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true))
                    {
                        target.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = (int)(3600 * target.GetStatValue(StatDefOf.PsychicSensitivity));
                    }
                }
                pawns.Clear();
            }

        }
        public override void PostDeSpawn(Map map)
        {
            if (parent.def.defName == "SZ_SeXie" && parent.Faction != Faction.OfPlayer)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("SZ_SeXieInsect"), null);
                pawn.TryGetComp<CompSeXieExpansion>().human = parent as Pawn;
                pawn.ageTracker.BirthAbsTicks = (parent as Pawn).ageTracker.BirthAbsTicks;
                GenSpawn.Spawn(pawn, parent.Position, map, Rot4.Random, WipeMode.Vanish, false);
            }
        }
        private int ticks = 3600;
        private static List<Pawn> pawns = new List<Pawn>();
        private List<Pawn> pawnsLocal = new List<Pawn>();
        private Pawn human;
        public static readonly Material Ring = MaterialPool.MatFrom("AncientChineseBeast/SeXie/Ring", ShaderDatabase.MoteGlow);
    }
}
