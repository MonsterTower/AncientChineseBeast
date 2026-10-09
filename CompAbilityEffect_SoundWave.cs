using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class CompAbilityEffect_SoundWave : CompAbilityEffect_SectorCells
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
            if (parent.pawn.def.defName == "SZ_MingShe")
            {
                parent.StartCooldown(0);
            }
            float angle = (target.CenterVector3 - parent.pawn.TrueCenter()).AngleFlat();
            FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.pawn.DrawPos, parent.pawn.Map, DefDatabase<FleckDef>.GetNamed("SZ_MingSheWave"), 1f);
            dataStatic.rotation = angle;
            float angle2 = Mathf.Atan2((float)(-(float)(target.CenterVector3.z - parent.pawn.Position.z)), (float)(target.CenterVector3.x - parent.pawn.Position.x)) * 57.29578f;
            // 1.6 inserted postExplosionGasRadiusOverride and postExplosionGasAmount in the middle of
            // DoExplosion's parameter list, so the all-positional call lands two slots off from the
            // fifteenth argument on. Named arguments say what is meant; every argument left out here
            // already passes the parameter's own default.
            GenExplosion.DoExplosion(parent.pawn.Position, parent.pawn.Map, 29.9f, DefDatabase<DamageDef>.GetNamed("SZ_SoundWaveDamage"), parent.pawn, 15, 100f, postExplosionSpawnThingCount: 0, preExplosionSpawnThingCount: 0, ignoredThings: new List<Thing>() { parent.pawn }, affectedAngle: new FloatRange(angle2 - 22.5f, angle2 + 22.5f), doVisualEffects: false, doSoundEffects: false, screenShakeFactor: 0f);
            parent.pawn.Map.flecks.CreateFleck(dataStatic);
        }
    }
}
