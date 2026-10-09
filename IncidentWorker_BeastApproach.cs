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
            BeastClass beast = Singleton.instance.BeastFor(def);
            Pawn pawn = PawnGenerator.GeneratePawn(beast.pawn, null);
            GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
            SendStandardLetter(beast.label, beast.text, def.letterDef, parms, pawn);
            return true;
        }
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, false, null);
        }
    }
}
