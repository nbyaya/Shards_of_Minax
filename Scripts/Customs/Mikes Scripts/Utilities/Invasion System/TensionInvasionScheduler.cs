using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Mobiles;
using Server.Commands;
using Bittiez.CustomSystems; // Ensure this matches your TensionManager namespace

namespace Server.Customs.Invasion_System
{
    public static class TensionInvasionScheduler
    {
        private static Timer _schedulerTimer;

        public static void Initialize()
        {
            // Check every hour (both initial delay and repeat interval set to 1 hour)
            _schedulerTimer = Timer.DelayCall(TimeSpan.FromHours(1), TimeSpan.FromHours(1), CheckAndScheduleInvasions);
			
            // Register admin command for manual checking
            CommandSystem.Register("CheckTensionInvasions", AccessLevel.Administrator, CheckTensionInvasions_OnCommand);			
        }

        [Usage("CheckTensionInvasions")]
        [Description("Manually checks the current tension and calculates how many towns would be invaded.")]
        public static void CheckTensionInvasions_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            int tension = TensionManager.Tension;

            from.SendMessage($"[Tension Debug] Current Tension: {tension}");

            if (tension < 10000)
            {
                from.SendMessage("[Tension Debug] Not enough tension to trigger an invasion. Minimum required: 10,000.");
                return;
            }

            // Calculate proportion based on tension (range: 10,000 to 100,000)
            double proportion = (tension - 10000) / 200000.0;
            if (proportion > 1.0)
                proportion = 1.0;

            int totalTowns = Enum.GetNames(typeof(InvasionTowns)).Length;
            int townsToInvade = (int)Math.Round(proportion * totalTowns);
            if (townsToInvade < 1)
                townsToInvade = 1;

            from.SendMessage($"[Tension Debug] Invasion Calculation: {townsToInvade} town(s) would be invaded.");

            // Show towns that are currently available for invasion
            List<InvasionTowns> availableTowns = new List<InvasionTowns>();
            foreach (InvasionTowns town in Enum.GetValues(typeof(InvasionTowns)))
            {
                bool isInvaded = InvasionControl.Invasions.Exists(inv => inv.InvasionTown == town);
                if (!isInvaded)
                    availableTowns.Add(town);
            }

            from.SendMessage($"[Tension Debug] Available towns for invasion: {availableTowns.Count}");
            
            // Optionally, allow the admin to trigger the invasion manually
            if (e.Arguments.Length > 0 && e.Arguments[0].ToLower() == "start")
            {
                from.SendMessage("[Tension Debug] Manually triggering invasions!");
                TriggerInvasions(townsToInvade);
            }
        }

        private static void TriggerInvasions(int townsToInvade)
        {
            List<InvasionTowns> availableTowns = new List<InvasionTowns>();
            foreach (InvasionTowns town in Enum.GetValues(typeof(InvasionTowns)))
            {
                bool isInvaded = InvasionControl.Invasions.Exists(inv => inv.InvasionTown == town);
                if (!isInvaded)
                    availableTowns.Add(town);
            }

            if (availableTowns.Count == 0)
                return;

            if (townsToInvade > availableTowns.Count)
                townsToInvade = availableTowns.Count;

            for (int i = 0; i < townsToInvade; i++)
            {
                int index = Utility.Random(availableTowns.Count);
                InvasionTowns selectedTown = availableTowns[index];
                availableTowns.RemoveAt(index);

                TownMonsterType monster = (TownMonsterType)Utility.Random(Enum.GetNames(typeof(TownMonsterType)).Length);
                TownChampionType champion = (TownChampionType)Utility.Random(Enum.GetNames(typeof(TownChampionType)).Length);
                DateTime startTime = DateTime.UtcNow;

                new TownInvasion(selectedTown, monster, champion, startTime);
            }
        }

        private static void CheckAndScheduleInvasions()
        {
            int tension = TensionManager.Tension;

            // Only trigger invasions if tension is at least 10,000
            if (tension < 10000)
                return;

            // Calculate proportion based on tension (range: 10,000 to 100,000)
            double proportion = (tension - 10000) / 90000.0;
            if (proportion > 1.0)
                proportion = 1.0;

            // Determine the total number of towns
            int totalTowns = Enum.GetNames(typeof(InvasionTowns)).Length;
            // Calculate how many towns to invade (at least one)
            int townsToInvade = (int)Math.Round(proportion * totalTowns);
            if (townsToInvade < 1)
                townsToInvade = 1;

            // Get a list of towns that aren’t currently invaded
            List<InvasionTowns> availableTowns = new List<InvasionTowns>();
            foreach (InvasionTowns town in Enum.GetValues(typeof(InvasionTowns)))
            {
                bool isInvaded = InvasionControl.Invasions.Exists(inv => inv.InvasionTown == town);
                if (!isInvaded)
                    availableTowns.Add(town);
            }

            // If every town is already invaded, do nothing (or handle as desired)
            if (availableTowns.Count == 0)
                return;

            // If the number to invade is more than available, limit it
            if (townsToInvade > availableTowns.Count)
                townsToInvade = availableTowns.Count;

            // For each invasion to schedule...
            for (int i = 0; i < townsToInvade; i++)
            {
                int index = Utility.Random(availableTowns.Count);
                InvasionTowns selectedTown = availableTowns[index];
                availableTowns.RemoveAt(index);

                // Choose random monster and champion types
                TownMonsterType monster = (TownMonsterType)Utility.Random(Enum.GetNames(typeof(TownMonsterType)).Length);
                TownChampionType champion = (TownChampionType)Utility.Random(Enum.GetNames(typeof(TownChampionType)).Length);

                // Start time is set to now (you could add a short delay if desired)
                DateTime startTime = DateTime.UtcNow;

                // Create the invasion
                new TownInvasion(selectedTown, monster, champion, startTime);
            }
        }
    }
}
