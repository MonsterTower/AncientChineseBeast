using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class DefModExtension_Beasts : DefModExtension
    {
        public List<BeastClass> beasts;
    }
    public class BeastClass : IExposable
    {
        public PawnKindDef pawn;
        [MustTranslate]
        public string text;
        [MustTranslate]
        public string label;

        public void ExposeData()
        {
            Scribe_Defs.Look(ref pawn, "pawn");
            Scribe_Values.Look(ref text, "text");
            Scribe_Values.Look(ref label, "label");
        }
    }
}
