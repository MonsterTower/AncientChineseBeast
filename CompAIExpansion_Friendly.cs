using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AncientChineseBeast
{
    public class CompAIExpansionFriendly : ThingComp
    {
        public CompProperties_AIExpansionFriendly Props => props as CompProperties_AIExpansionFriendly;

        public override void CompTick()
        {
            Pawn pawn = parent as Pawn;
            if (pawn.CurJobDef == RimWorld.JobDefOf.AttackMelee)
            {
                if (pawn.CurJob.targetA.Pawn is Pawn targetNow)
                {
                    if (pawn.abilities.GetAbility(Props.rangeAbility)is Ability ability && pawn.Position.Standable(pawn.Map) && ability.CanCast)
                    {
                        if (ability.verb.CanHitTarget(pawn.mindState.enemyTarget))
                        {
                            pawn.jobs.StartJob(ability.GetJob(pawn.mindState.enemyTarget, pawn.mindState.enemyTarget));
                        }
                    }
                }
            }
        }
    }
}
