using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace AncientChineseBeast
{
    public class CompAbilityEffect_Crow : CompAbilityEffect
    {
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            return true;
        }
        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return true;
        }
        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            return true;
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Map map = parent.pawn.Map;
            SoundDef.Named("SZ_Crow").PlayOneShot(parent.pawn);
            foreach (var x in map.mapPawns.AllPawns)
            {
                if (x.Faction != null && (x.Faction.IsPlayer || x.Faction.PlayerRelationKind == FactionRelationKind.Ally) && x.needs != null && x.needs.mood != null && x.needs.mood.thoughts != null && x.needs.mood.thoughts.memories != null)
                {
                    x.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("SZ_ChickenCrow"), null, null);
                }
                if (x.def.defName == "SZ_SeXie" || x.def.defName == "SZ_SeXieInsect")
                {
                    pawns.Add(x);
                }
            }
            if (pawns.Count > 0)
            {
                Pawn index = pawns.Last();
                index.Kill(null);
                pawns.RemoveLast();
            }
        }
        private static List<Pawn> pawns = new List<Pawn>();
    }
}
