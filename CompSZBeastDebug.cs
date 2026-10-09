using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Verse;

namespace AncientChineseBeast
{
    // Development-mode entries, replacing the three buttons the original hung on the firecracker's
    // own ThingComp (CompSZBeastDebug, removed together with its def entry). Two of those buttons
    // did nothing: they changed Singleton.nextBeastTimeHours, which nothing reads. What decides when
    // a beast comes is Singleton.HourTick (a 1% roll once a day, refused until 3 600 000 ticks have
    // passed since the last one), so "sooner" is two levers, kept separate here.
    public static class DebugActions
    {
        [DebugActionYielder]
        private static IEnumerable<DebugActionNode> LocalizedActions()
        {
            yield return LocalizedAction("SZ_DebugBeastNow", ForceBeastApproach);
            yield return LocalizedAction("SZ_DebugNianNextHour", ForceYearBeast);
            yield return LocalizedAction("SZ_DebugClearCooldown", ClearBeastCooldown);
            yield return LocalizedAction("SZ_DebugReportClock", ReportBeastClock);
        }

        private static DebugActionNode LocalizedAction(string key, Action action)
        {
            // Attribute arguments must be constants and the game does not translate them.
            // Yield nodes directly so both the category and the visible labels are localized.
            return new DebugActionNode(key, action: action)
            {
                category = "SZ_DebugCategory".Translate(),
                labelGetter = () => key.Translate(),
                sourceAttribute = new DebugActionAttribute { allowedGameStates = AllowedGameStates.PlayingOnMap }
            };
        }

        // Sixty days, the interval HourTick requires between two beasts under any storyteller but Sexie.
        private const int BeastCooldownTicks = 3600000;

        private static void ForceBeastApproach()
        {
            Singleton.instance.BeastApproach(forced: true);
        }

        private static void ForceYearBeast()
        {
            Singleton.instance.YearBeastForced = true;
            Messages.Message("SZ_DebugNianScheduled".Translate(), MessageTypeDefOf.TaskCompletion, historical: false);
        }

        // The roll still has to come up, so this is not "a beast now".
        private static void ClearBeastCooldown()
        {
            Singleton.instance.lastBeastTime = Find.TickManager.TicksGame - BeastCooldownTicks - 1;
            Messages.Message("SZ_DebugCooldownCleared".Translate(), MessageTypeDefOf.TaskCompletion, historical: false);
        }

        private static void ReportBeastClock()
        {
            int now = Find.TickManager.TicksGame;
            int since = now - Singleton.instance.lastBeastTime;
            string storyteller = Find.Storyteller.def.defName;
            string schedule = storyteller == "SZ_Storyteller_Sexie"
                ? "Sexie: one beast every 900000 ticks, no roll"
                : $"ordinary storyteller: 1% once a day, refused until {BeastCooldownTicks} ticks have passed";
            Log.Message(
                $"[Ancient Chinese Beast] tick {now}, {since} ticks since the last beast, storyteller {storyteller} ({schedule}). "
                + $"Chosen beast: {Singleton.instance.beast?.pawn?.defName ?? "none yet"}. "
                + $"Corpses held: {Singleton.instance.beastCorpses.Count} kind(s).");
            Messages.Message("SZ_DebugClockLogged".Translate(), MessageTypeDefOf.TaskCompletion, historical: false);
        }
    }
}
