using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    public class CompCausePermanentGameCondition : ThingComp
    {
        public CompProperties_CausePermanentGameCondition Props => props as CompProperties_CausePermanentGameCondition;
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if(parent.Faction == Faction.OfPlayer)
            {
                return;
            }
            GameCondition gameCondition = GameConditionMaker.MakeConditionPermanent(Props.conditionDef);
            condition = gameCondition;
            gameCondition.conditionCauser = this.parent;
            parent.Map.gameConditionManager.RegisterCondition(gameCondition);
        }
        // 1.6 added a DestroyMode to ThingComp.PostDeSpawn; the one-argument form is never called
        // and would leave the permanent game condition running after its causer is gone.
        public override void PostDeSpawn(Map map, DestroyMode mode)
        {
            condition?.End();
        }
        public override void PostExposeData()
        {
            Scribe_References.Look(ref condition, "causedCondition");
        }
        private GameCondition condition;
    }
}
