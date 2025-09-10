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
        public override void PostDeSpawn(Map map)
        {
            condition.End();
        }
        public override void PostExposeData()
        {
            Scribe_References.Look(ref condition, "causedCondition");
        }
        private GameCondition condition;
    }
}
