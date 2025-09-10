using RimWorld;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class Ability_Draw : Ability
    {
        public Ability_Draw(Pawn pawn) : base(pawn)
        {
        }


        public Ability_Draw(Pawn pawn, AbilityDef def) : base(pawn, def)
        {
        }
        public virtual void Draw(Vector3 drawLoc)
        {

        }
    }
}
