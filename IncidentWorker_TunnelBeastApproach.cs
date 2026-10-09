using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace AncientChineseBeast
{
    public class IncidentWorker_TunnelBeastApproach : IncidentWorker
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
            BeastClass beast = Singleton.instance.BeastFor(def);
            Thing thing = GenSpawn.Spawn(ThingDef.Named("SZ_SeXieTunnelSpawner"), intVec, map, WipeMode.Vanish);
            SendStandardLetter(beast.label, beast.text, def.letterDef, parms, thing);
            return true;
        }
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            // AllRooms also carries the map's great outdoors as one giant, unenclosed "room": on a poor colony its
            // scattered rocks and chunks can out-value every built room, and a random cell of it can land anywhere on
            // the map, never seen by a colonist. ProperRoom (well enclosed) rules that out, but not a sealed ancient
            // vault (a ruin the base game scatters underground, walled off, never opened): it is well enclosed too and
            // its loot (a sarcophagus, ultratech medicine, a hive) can be worth more than the colony's own rooms, and a
            // cell inside it is never seen by a colonist either. Restricted to a room that touches the home area: the
            // player's own declared territory, not a ruin nobody has found yet.
            rooms.AddRange(map.regionGrid.AllRooms.Where((Room r) => r.ProperRoom && r.Cells.Any((IntVec3 c) => map.areaManager.Home[c])));
            if (rooms.Count > 0)
            {
                rooms.SortBy((Room x) => x.GetStat(RoomStatDefOf.Wealth));
                rooms[rooms.Count - 1].Cells.TryRandomElement(out cell);
                rooms.Clear();
                return true;
            }
            cell = IntVec3.Invalid;
            return false;
        }
        List<Room> rooms = new List<Room>();
    }

}
