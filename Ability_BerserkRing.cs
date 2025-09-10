using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class Ability_BerserkRing : Ability_Draw
    {
        public Ability_BerserkRing(Pawn pawn) : base(pawn)
        {
        }
        public Ability_BerserkRing(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
        }
        public override void AbilityTick()
        {
            base.AbilityTick();
            if (ticksTime > 0)
            {
                ticksTime--;
                if (ticksTime % 180 == 0)
                {
                    pawnsLocal.Clear();
                    pawnsLocal.AddRange(pawn.Map.mapPawns.AllPawnsSpawned.Where((Pawn x) => x.RaceProps.Humanlike && pawn.HostileTo(x)));
                }
                foreach (var x in pawnsLocal)
                {
                    if (x.Spawned && !x.Downed && x.Position.InHorDistOf(pawn.Position, 10.9f))
                    {
                        float v = x.GetStatValue(StatDefOf.PsychicSensitivity);
                        if (v > 0 && x.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true))
                        {
                            x.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = (int)(3600 * v);
                        }
                    }
                }
            }
        }

        public override void Draw(Vector3 drawLoc)
        {
            if (ticksTime < 1)
            {
                return;
            }
            Matrix4x4 matrixD = default;
            matrixD.SetTRS(pawn.DrawPos, Quaternion.AngleAxis(0, Vector3.up), new Vector3(10.9f * 2.1f, 1f, 10.9f * 2.1f));
            Graphics.DrawMesh(MeshPool.plane10, matrixD, CompSeXieExpansion.Ring, 0);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksTime, "ticksTime");
        }
        public int ticksTime;
        private List<Pawn> pawnsLocal = new List<Pawn>();
    }
}
