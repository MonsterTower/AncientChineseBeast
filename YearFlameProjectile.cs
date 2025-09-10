using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class YearFlameProjectile : Projectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);
            if (hitThing != null)
            {
                hitThing.TakeDamage(new DamageInfo(def.projectile.damageDef, DamageAmount, ArmorPenetration, ExactRotation.eulerAngles.y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, intendedTarget.Thing, !(launcher is Pawn pawn) || !pawn.Drafted, true));
                hitThing.TryAttachFire(1,launcher);
                return;
            }
        }
    }
}
