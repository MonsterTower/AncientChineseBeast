using RimWorld;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class CompAbilityEffect_JetFlame : CompAbilityEffect_SectorCells
    {
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return target != null && target.Pawn != null;
        }
        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return true;
        }
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return true;
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            YearFlame thing = (YearFlame)GenSpawn.Spawn(ThingDef.Named("SZ_YearBeastFlame"), parent.pawn.Position, parent.pawn.Map, WipeMode.Vanish);
            thing.launcher = parent.pawn;
            Vector3 pos = parent.pawn.TrueCenter();
            if (parent.pawn.def.defName == "SZ_YearBeast" || parent.pawn.def.defName == "SZ_YearBeast_Friendly")
            {
                parent.StartCooldown(0);
                switch (parent.pawn.Rotation.AsInt)
                {
                    case 0:
                        pos.z += 3.75f;
                        break;
                    case 1:
                        pos.x += 4.2f;
                        break;
                    case 2:
                        pos.z -= 3f;
                        break;
                    default:
                        pos.x -= 4.2f;
                        break;

                }
            }
            thing.origin = pos;
            thing.angle = (target.CenterVector3 - thing.origin).AngleFlat();

        }
    }
}
