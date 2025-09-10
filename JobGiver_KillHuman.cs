using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public abstract class JobGiver_KillHuman : ThinkNode_JobGiver
    {
        protected static List<Pawn> targetsCacheReachable = new List<Pawn>();
        protected static List<Pawn> targetsCacheUnreachable = new List<Pawn>();
        protected Pawn FindPawnTarget(Pawn pawn)
        {

            float distance = 99999f;
            Pawn result = null;
            foreach (var x in pawn.Map.mapPawns.AllPawns)
            {
                if (!x.Downed && ((x.Faction != null && x.Faction.IsPlayer) || x.RaceProps.Humanlike))
                {
                    if (pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly))
                    {
                        targetsCacheReachable.Add(x);
                    }
                    else
                    {
                        targetsCacheUnreachable.Add(x);

                    }
                }
            }
            var list = targetsCacheReachable.Count > 0 ? targetsCacheReachable : targetsCacheUnreachable;
            foreach (var x in list)
            {
                float px = (pawn.Position.x - x.Position.x);
                float pz = (pawn.Position.z - x.Position.z);
                float distanceB = px * px + pz * pz;
                if (distanceB < distance)
                {
                    distance = distanceB;
                    result = x;

                }
            }
            targetsCacheReachable.Clear();
            targetsCacheUnreachable.Clear();
            return result;
        }
    }
}