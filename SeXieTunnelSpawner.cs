using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class SeXieTunnelSpawner : TunnelHiveSpawner
    {
        protected override void Spawn(Map map, IntVec3 loc)
        {
            PawnKindDef thrumbo = Singleton.instance.beast.pawn;
            Pawn pawn = PawnGenerator.GeneratePawn(thrumbo, null);
            GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
        }
    }
}
