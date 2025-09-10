using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace AncientChineseBeast
{
    public class Verb_MeleeAttackDamage_HitHead : Verb_MeleeAttackDamage
    {
        protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
        {
            if (Rand.Chance(0.5f))
            {
                return base.ApplyMeleeDamageToTarget(target);
            }
            foreach (DamageInfo dinfo in typeof(Verb_MeleeAttackDamage).GetMethod("DamageInfosToApply", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, new object[] { target }) as IEnumerable<DamageInfo>)
            {
                if (target.ThingDestroyed)
                {
                    break;
                }
                if (target.Pawn != null)
                {
                    BodyPartRecord bodyPart = target.Pawn.def.race.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource)?.First();
                    if (bodyPart != null)
                    {
                        while (bodyPart.depth != BodyPartDepth.Outside)
                        {
                            bodyPart = bodyPart.parent;
                        }
                        foreach (Hediff x in target.Pawn.health.hediffSet.hediffs)
                        {
                            if (x is Hediff_MissingPart && x.Part == bodyPart)
                            {
                                return target.Thing.TakeDamage(dinfo);
                            }
                        }
                        dinfo.SetHitPart(bodyPart);
                    }
                }
                return target.Thing.TakeDamage(dinfo);
            }
            return new DamageWorker.DamageResult();
        }
    }
}
