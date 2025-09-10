using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class PawnFlyingStrike : PawnFlyer
    {
        public override void Tick()
        {
            DoubleTick();
            if (Spawned)
            {
                DoubleTick();
            }
        }

        private void DoubleTick()
        {
            int num = GenRadial.NumCellsInRadius(3.9f);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = groundPos.ToIntVec3() + GenRadial.RadialPattern[i];
                if (intVec.InBounds(Map))
                {
                    foreach (Thing x in intVec.GetThingList(Map))
                    {
                        if ((FlyingPawn.HostileTo(x) || FlyingPawn.def.defName == "SZ_QiongQi") && !hurtedTargets.Contains(x))
                        {
                            hurtedTargets.Add(x);
                            targets.Add(x);
                        }
                    }
                }
            }
            while (targets.Count > 0)
            {
                Thing target = targets[targets.Count - 1];
                BodyPartRecord bodyPart = null;
                if (target is Pawn && Rand.Chance(0.5f))
                {
                    bodyPart = target.def.race.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource)?.First();
                }
                target.TakeDamage(new DamageInfo(DamageDefOf.Cut, 25, 1, -1, FlyingPawn, bodyPart));
                targets.RemoveAt(targets.Count - 1);
            }
            RecomputePosition();
            base.Tick();
        }

        private void RecomputePosition()
        {
            if (positionLastComputedTick == ticksFlying)
            {
                return;
            }
            if (angle == -1f)
            {
                angle = (DestinationPos - startVec).AngleFlat();
            }
            positionLastComputedTick = ticksFlying;
            float arg = ticksFlying / (float)ticksFlightTime;
            groundPos = Vector3.Lerp(startVec, DestinationPos, arg);
        }
        public override void DynamicDrawPhaseAt(DrawPhase phase,Vector3 drawLoc, bool flip = false)
        {
            RecomputePosition();
            FlyingPawn.DynamicDrawPhaseAt(phase,groundPos, flip);
        }
        private int positionLastComputedTick = -1;
        private Vector3 groundPos;
        private float angle = -1f;
        public HashSet<Thing> hurtedTargets = new HashSet<Thing>();
        private static List<Thing> targets = new List<Thing>();
        public static PawnFlyingStrike Make(ThingDef flyingDef, Ability ability, Pawn pawn, IntVec3 destCell, EffecterDef flightEffecterDef, SoundDef landingSound, bool flyWithCarriedThing = false)
        {
            pawn.rotationTracker.FaceCell(destCell);
            return MakeFlyer(flyingDef, pawn, destCell, flightEffecterDef, landingSound, flyWithCarriedThing) as PawnFlyingStrike;
        }
    }
}
