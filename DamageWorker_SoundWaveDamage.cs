using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class DamageWorker_SoundWaveDamage : DamageWorker_AddInjury
    {
        public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
            if (damageResult.totalDamageDealt > 0 && victim.Spawned)
            {
                victim.TakeDamage(new DamageInfo(DamageDefOf.Stun, 4));

            }
            return damageResult;
        }
    }
}
