using RimWorld;
using System;
using Verse;

namespace AncientChineseBeast
{
    public class IncidentWorker_ChickenPasses : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 intVec;
            return this.TryFindEntryCell(map, out intVec);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 intVec;
            if (!this.TryFindEntryCell(map, out intVec))
            {
                return false;
            }
            PawnKindDef thrumbo = PawnKindDef.Named("SZ_Chicken");
            int num2 = Rand.RangeInclusive(90000, 150000);
            IntVec3 invalid = IntVec3.Invalid;
            if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(intVec, map, 10f, out invalid))
            {
                invalid = IntVec3.Invalid;
            }
            Pawn pawn = null;
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
            pawn = PawnGenerator.GeneratePawn(thrumbo, null);
            GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
            pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num2;
            if (invalid.IsValid)
            {
                pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(invalid, map, 10, null);
            }
            base.SendStandardLetter(parms, pawn, Array.Empty<NamedArgument>());
            return true;
        }

        // Token: 0x060055D3 RID: 21971 RVA: 0x001CD9A0 File Offset: 0x001CBBA0
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, false, null);
        }
    }
}
