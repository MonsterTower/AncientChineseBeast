using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public static class Cells
    {
        public static List<IntVec3> EightCells = new List<IntVec3> {
                new IntVec3(0,0,1),
                new IntVec3(0,0,-1),
                new IntVec3(1,0,0),
                new IntVec3(-1,0,0),
                new IntVec3(1,0,1),
                new IntVec3(1,0,-1),
                new IntVec3(-1,0,1),
                new IntVec3(-1,0,-1),
            };
    }
}
