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
        // Which beast the incident is about. Fired any way other than BeastApproach (e.g. the development menu), `beast` is null.
        // Each incident def carries its own list in a DefModExtension_Beasts, so there is always an answer available.
        public BeastClass BeastFor(IncidentDef incident)
        {
            var extension = incident?.GetModExtension<DefModExtension_Beasts>();
            // Older saves contain a deep copy of the letter in the language used at save time.
            // Resolve the same pawn kind against the current Def so a language change takes effect.
            if (beast != null && extension != null)
            {
                BeastClass current = extension.beasts.Find(candidate => candidate.pawn == beast.pawn);
                if (current != null) beast = current;
            }
            if (beast == null)
            {
                if (extension != null && extension.beasts.Count > 0) beast = extension.beasts.RandomElement();
            }
            return beast;
        }
        // forced is for the development action: an incident that is not forced answers to the storyteller's difficulty
        // and to its own conditions, and the action did nothing at all when CanFireNow said no.
        public void BeastApproach(bool forced = false)
        {
            Map map = Find.Maps.FindAll((Map x) => x.IsPlayerHome).RandomElement();
            IncidentDef incidentA = IncidentDef.Named("SZ_BeastApproach");
            IncidentDef incidentB = IncidentDef.Named("SZ_BeastApproachTunnel");
            var A = incidentA.GetModExtension<DefModExtension_Beasts>().beasts;
            var B = incidentB.GetModExtension<DefModExtension_Beasts>().beasts;
            int index = Rand.Range(1, A.Count + B.Count + 1);
            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, map);
            parms.forced = forced;
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
