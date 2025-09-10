using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AncientChineseBeast
{
    public class Singleton : IExposable
    {
        public static Singleton instance = new Singleton();
        public void Tick()
        {
            if (Find.TickManager.TicksGame % 2500 == 0)
            {
                HourTick();
            }
        }
        public void HourTick()
        {
            nextBeastTimeHours++;
            if (Find.Storyteller.def.defName == "SZ_Storyteller_Sexie")
            {
                if (Find.TickManager.TicksGame > 60000 && (Find.TickManager.TicksGame) % (15 * 60000) == 0)
                {
                    BeastApproach();
                }
            }
            else
            if (Find.TickManager.TicksGame % 60000 == 0 && Find.TickManager.TicksGame - lastBeastTime > 60000 * 60)
            {
                if (Rand.Chance(0.01f))
                {
                    lastBeastTime = Find.TickManager.TicksGame;
                    BeastApproach();
                }
            }
            foreach (var map in Find.Maps.FindAll((Map x) => x.IsPlayerHome))
            {
                Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);

                if (YearBeastForced || GenLocalDate.HourOfDay(map) == 0 && GenDate.DayOfYear(Find.TickManager.TicksAbs, vector.x) == 0)
                {
                    IncidentDef def = IncidentDef.Named("SZ_YearBeastApproach");
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(def.category, map);
                    parms.forced = true;
                    if (def.Worker.CanFireNow(parms))
                    {
                        lastBeastTime = Find.TickManager.TicksGame;
                        YearBeastForced = false;
                        def.Worker.TryExecute(parms);

                    }
                }
            }
        }
        public void BeastApproach()
        {
            Map map = Find.Maps.FindAll((Map x) => x.IsPlayerHome).RandomElement();
            IncidentDef incidentA = IncidentDef.Named("SZ_BeastApproach");
            IncidentDef incidentB = IncidentDef.Named("SZ_BeastApproachTunnel");
            var A = incidentA.GetModExtension<DefModExtension_Beasts>().beasts;
            var B = incidentB.GetModExtension<DefModExtension_Beasts>().beasts;
            int index = Rand.Range(1, A.Count + B.Count + 1);
            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, map);
            if (index > A.Count)
            {
                beast = B[index - A.Count - 1];
                if (incidentB.Worker.CanFireNow(parms))
                {
                    nextBeastTimeHours = 0;
                    incidentB.Worker.TryExecute(parms);
                }
            }
            else
            {
                beast = A[index - 1];
                if (incidentA.Worker.CanFireNow(parms))
                {
                    nextBeastTimeHours = 0;
                    incidentA.Worker.TryExecute(parms);
                }
            }
        }
        public void ExposeData()
        {
            Scribe_Deep.Look(ref beast, "beast");
            Scribe_Values.Look(ref nextBeastTimeHours, "nextBeastTimeHours");
            Scribe_Values.Look(ref lastBeastTime, "lastYearBeast");
        }
        public Dictionary<ThingDef, List<Corpse>> beastCorpses = new Dictionary<ThingDef, List<Corpse>>();
        public BeastClass beast;
        public int lastBeastTime;
        public int nextBeastTimeHours = -60 * 24;
        public bool YearBeastForced;
    }
}
