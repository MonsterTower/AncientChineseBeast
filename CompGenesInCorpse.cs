using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AncientChineseBeast
{
    public class CompGenesInCorpse : ThingComp
    {
        CompProperties_GenesInCorpse Props
        {
            get
            {
                return (CompProperties_GenesInCorpse)this.props;
            }
        }
        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (mode == DestroyMode.KillFinalize)
            {
                if (!Singleton.instance.beastCorpses.ContainsKey(parent.def))
                {
                    Singleton.instance.beastCorpses.Add(parent.def, new List<Corpse>());
                }
                Singleton.instance.beastCorpses[parent.def].Add((parent as Pawn).Corpse);
            }
        }

        public void ExtractAt(IntVec3 position, Map previousMap)
        {
            Genepack genepack = (Genepack)ThingMaker.MakeThing(ThingDefOf.Genepack, null);
            List<GeneDef> genesToAdd = new List<GeneDef>() { Props.genes.RandomElement() };
            genepack.Initialize(genesToAdd);
            GenPlace.TryPlaceThing(genepack, position, previousMap, ThingPlaceMode.Near);
            (parent as Pawn).Corpse.Destroy();
            Singleton.instance.beastCorpses.TryGetValue(parent.def, out List<Corpse> corpses);
            corpses.Remove((parent as Pawn).Corpse);
        }
    }
}
