using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class Verb_MeleeAttackDamage_Area : Verb_MeleeAttackDamage
    {
        private IEnumerable<Thing> GetTargets(IntVec3 pos)
        {
            int num = GenRadial.NumCellsInRadius(3f);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = pos + GenRadial.RadialPattern[i];
                if (intVec.InBounds(caster.Map))
                {
                    foreach (Thing y in intVec.GetThingList(Caster.Map))
                    {
                        if (CanHitTarget(y))
                        {
                            yield return y;
                        }
                    }
                }
            }
        }
        public override bool CanHitTarget(LocalTargetInfo targ)
        {
            return this.caster != null && this.caster.Spawned && targ != this.caster;
        }
        protected override bool TryCastShot()
        {
            LocalTargetInfo targetInfo = currentTarget;
            bool result = false;
            targets.AddRange(GetTargets(targetInfo.Cell));
            while (targets.Count > 0)
            {
                var x = targets[targets.Count - 1];
                if (CanHitTarget(x))
                {
                    currentTarget = x;
                    if (base.TryCastShot())
                    {
                        result = true;
                    }
                    targets.RemoveAt(targets.Count - 1);
                }
            }
            return result;
        }
        protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
        {
            var x = base.ApplyMeleeDamageToTarget(target);
            x.deflected = false;
            return x;
        }
        private static List<Thing> targets = new List<Thing>();
    }
}
