using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class IncidentWorker_BeastApproach : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return TryFindEntryCell(map, out _);
        }
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            if (!TryFindEntryCell(map, out IntVec3 intVec))
            {
                return false;
            }
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
            Pawn pawn = PawnGenerator.GeneratePawn(Singleton.instance.beast.pawn, null);
            GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
            SendStandardLetter(Singleton.instance.beast.label, Singleton.instance.beast.text, def.letterDef, parms, pawn);
            return true;
        }
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, false, null);
        }
    }
}
