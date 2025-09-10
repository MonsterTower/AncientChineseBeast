using Verse;

namespace AncientChineseBeast
{
    public class Firecracker : Thing
    {
        private float SpreadRange
        {
            get
            {
                switch (damageDef.defName)
                {
                    case "SZ_Firecracker_Flame":
                        return 4.7f;
                    case "SZ_Firecracker_FlameB":
                        return 3.5f;
                    default:
                        return 1.4f;
                }
            }
        }
        private string NextLevel
        {
            get
            {
                switch (damageDef.defName)
                {
                    case "SZ_Firecracker_Flame":
                        return "SZ_Firecracker_FlameB";
                    case "SZ_Firecracker_FlameB":
                        return "SZ_Firecracker_FlameC";
                    default:
                        return "";
                }
            }
        }
        public override void Tick()
        {
            if (timingTicks == -1)
            {
                if (damageDef.defName == "SZ_Firecracker_Flame")
                {
                    timingTicks = Rand.Range(3, 10);
                }
                else
                {
                    timingTicks = Rand.Range(15, 55);
                }
            }
            timingTicks--;
            if (timingTicks == 0)
            {
                DoExplosion();
            }
        }
        private void DoExplosion()
        {
            GenExplosion.DoExplosion(Position, Map, SpreadRange, DefDatabase<DamageDef>.GetNamed(NextLevel), instigator, -1, -1, SoundDef.Named("SZ_Firecracker"), weapon, projectile, null, null, 0, 0, null, false, null, 0, 0, 0, false, null, null, null, false);
            Destroy();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref damageDef, "damageDef");
            Scribe_Values.Look(ref timingTicks, "timingTicks", -1);
            Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
            Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
            Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
        }
        private int timingTicks = -1;
        private DamageDef damageDef;
        private Thing instigator;
        private ThingDef weapon;
        private ThingDef projectile;

        public static void SpawnFirecracker(IntVec3 pos, Map map, DamageDef damageDef, Thing instigator, ThingDef weapon, ThingDef projectile)
        {
            Firecracker firecracker = (Firecracker)GenSpawn.Spawn(ThingDef.Named("SZ_Firecracker_Explosion"), pos, map, WipeMode.Vanish);
            firecracker.damageDef = damageDef;
            firecracker.instigator = instigator;
            firecracker.weapon = weapon;
            firecracker.projectile = projectile;
        }
    }
}
