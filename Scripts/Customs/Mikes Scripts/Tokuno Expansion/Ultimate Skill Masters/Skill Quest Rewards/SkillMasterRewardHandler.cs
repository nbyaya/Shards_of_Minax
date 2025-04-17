using Server.ACC.CSS.Systems.AlchemyMagic;
using Server.ACC.CSS.Systems.FishingMagic;
using Server.ACC.CSS.Systems.EvalIntMagic;
using Server.ACC.CSS.Systems.ArcheryMagic;
using Server.ACC.CSS.Systems.MageryMagic;
using Server.ACC.CSS.Systems.ArmsLoreMagic;
using Server.ACC.CSS.Systems.AnimalTamingMagic;
using Server.ACC.CSS.Systems.AnimalLoreMagic;
using Server.ACC.CSS.Systems.CarpentryMagic;
using Server.ACC.CSS.Systems.CartographyMagic;
using Server.ACC.CSS.Systems.TasteIDMagic;
using Server.ACC.CSS.Systems.CookingMagic;
using Server.ACC.CSS.Systems.DiscordanceMagic;
using Server.ACC.CSS.Systems.FencingMagic;
using Server.ACC.CSS.Systems.FletchingMagic;
using Server.ACC.CSS.Systems.ForensicsMagic;
using Server.ACC.CSS.Systems.WrestlingMagic;
using Server.ACC.CSS.Systems.ParryMagic;
using Server.ACC.CSS.Systems.HealingMagic;
using Server.ACC.CSS.Systems.DetectHiddenMagic;
using Server.ACC.CSS.Systems.ProvocationMagic;
using Server.ACC.CSS.Systems.LockpickingMagic;
using Server.ACC.CSS.Systems.MacingMagic;
using Server.ACC.CSS.Systems.MeditationMagic;
using Server.ACC.CSS.Systems.BeggingMagic;
using Server.ACC.CSS.Systems.MiningMagic;
using Server.ACC.CSS.Systems.ChivalryMagic;
using Server.ACC.CSS.Systems.StealingMagic;
using Server.ACC.CSS.Systems.InscribeMagic;
using Server.ACC.CSS.Systems.NinjitsuMagic;
using Server.ACC.CSS.Systems.HidingMagic;
using Server.ACC.CSS.Systems.StealthMagic;
using Server.ACC.CSS.Systems.BlacksmithMagic;
using Server.ACC.CSS.Systems.TacticsMagic;
using Server.ACC.CSS.Systems.SwordsMagic;
using Server.ACC.CSS.Systems.TailoringMagic;
using Server.ACC.CSS.Systems.NecromancyMagic;
using Server.ACC.CSS.Systems.TrackingMagic;
using Server.ACC.CSS.Systems.RemoveTrapMagic;
using Server.ACC.CSS.Systems.VeterinaryMagic;
using Server.ACC.CSS.Systems.MusicianshipMagic;
using Server.ACC.CSS.Systems.CampingMagic;
using Server.ACC.CSS.Systems.LumberjackingMagic;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Server.Commands.Generic;
using Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters.Skill_Quest_Rewards;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters
{
    public interface IRewardGroup
    {
        int Weight { get; }
        Type[] GetRewards(PlayerMobile player, SkillName? skill);
    }
    public static class RewardManager
    {
        private static readonly Dictionary<SkillName, string> SkillMasterTitles = new Dictionary<SkillName, string>
        {
            { SkillName.Alchemy, "Alchemy Master" },
            { SkillName.AnimalLore, "Animal Lore Master" },
            { SkillName.AnimalTaming, "Animal Taming Master" },
            { SkillName.Archery, "Archery Master" },
            { SkillName.ArmsLore, "Arms Lore Master" },
            { SkillName.Begging, "Begging Master" },
            { SkillName.Blacksmith, "Blacksmithing Master" },
            { SkillName.Bushido, "Bushido Master" },
            { SkillName.Camping, "Camping Master" },
            { SkillName.Carpentry, "Carpentry Master" },
            { SkillName.Cartography, "Cartography Master" },
            { SkillName.Chivalry, "Chivalry Master" },
            { SkillName.Cooking, "Cooking Master" },
            { SkillName.DetectHidden, "Detect Hidden Master" },
            { SkillName.Discordance, "Discordance Master" },
            { SkillName.EvalInt, "Eval Intelligence Master" },
            { SkillName.Fencing, "Fencing Master" },
            { SkillName.Fishing, "Fishing Master" },
            { SkillName.Fletching, "Fletching Master" },
            { SkillName.Forensics, "Forensics Master" },
            { SkillName.Healing, "Healing Master" },
            { SkillName.Hiding, "Hiding Master" },
            { SkillName.Inscribe, "Inscribe Master" },
            { SkillName.Lockpicking, "Lockpicking Master" },
            { SkillName.Lumberjacking, "Lumberjacking Master" },
            { SkillName.Macing, "Macing Master" },
            { SkillName.Magery, "Magery Master" },
            { SkillName.Meditation, "Meditation Master" },
            { SkillName.Mining, "Mining Master" },
            { SkillName.Musicianship, "Musicianship Master" },
            { SkillName.Necromancy, "Necromancy Master" },
            { SkillName.Ninjitsu, "Ninjitsu Master" },
            { SkillName.Parry, "Parry Master" },
            { SkillName.Provocation, "Provocation Master" },
            { SkillName.RemoveTrap, "Remove Trap Master" },
            { SkillName.Stealing, "Stealing Master" },
            { SkillName.Stealth, "Stealth Master" },
            { SkillName.Swords, "Swordsmanship Master" },
            { SkillName.Tactics, "Tactics Master" },
            { SkillName.Tailoring, "Tailoring Master" },
            { SkillName.TasteID, "Taste Identification Master" },
            { SkillName.Tracking, "Tracking Master" },
            { SkillName.Veterinary, "Veterinary Master" },
            { SkillName.Wrestling, "Wrestling Master" }
        };

        private static readonly List<IRewardGroup> RewardGroups = new List<IRewardGroup>
        {
            new QuestRewardArmor(),
            new QuestRewardDeedItems(),
            new QuestRewardNonSummoningMateria(),
            new QuestRewardRandomJewelry(),
            new QuestRewardSkillSpecific(),
            new QuestRewardSummoningMateria(),
        };

        // Weighted selection logic for reward groups
        private static IRewardGroup SelectRewardGroup()
        {
            int totalWeight = RewardGroups.Sum(group => group.Weight);
            int randomWeight = new Random().Next(0, totalWeight);
            int cumulativeWeight = 0;

            foreach (var group in RewardGroups)
            {
                cumulativeWeight += group.Weight;
                if (randomWeight < cumulativeWeight)
                {
                    return group;
                }
            }

            return null; // Should never happen if weights are configured correctly
        }

        public static void HandleQuestReward(PlayerMobile player, SkillName skill, int questLevel)
        {
            // Configurable limit for extra rewards
            const int maxExtraRewards = 3; // Maximum number of extra rewards (not counting the first)

            // Give base rewards
            GiveBaseRewards(player, questLevel);

            // Give a PowerScroll reward
            GivePowerScrollReward(player, skill);

            // Select a reward group and give rewards
            IRewardGroup selectedGroup = SelectRewardGroup();
            if (selectedGroup != null)
            {
                var rewards = selectedGroup.GetRewards(player, skill);
                if (rewards.Length > 0)
                {
                    Random rng = new Random();
                    double rewardChance = 1.0; // Start with a 100% chance for the first reward
                    int extraRewardCount = 0; // Tracks the number of extra rewards given

                    // Loop through rewards to possibly award multiple items
                    while (rewardChance > 0.2 && extraRewardCount < maxExtraRewards)
                    {
                        // Select a random reward
                        Type selectedReward = rewards[rng.Next(0, rewards.Length)];

                        try
                        {
                            var rewardInstance = Activator.CreateInstance(selectedReward) as Server.Item;
                            if (rewardInstance != null)
                            {
                                player.AddToBackpack(rewardInstance);
                                Console.WriteLine($"Added {rewardInstance.GetType().Name} to {player.Name}'s backpack.");
                                extraRewardCount++; // Increment the extra reward counter
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error creating reward instance of {selectedReward.Name}: {ex.Message}");
                        }

                        // Reduce the chance for additional rewards
                        rewardChance *= 0.5;

                        // Break the loop if RNG fails the probability check
                        if (rng.NextDouble() > rewardChance)
                        {
                            break;
                        }
                    }
                }
            }

            // Log reward processing for debugging
            Console.WriteLine($"Reward processed for {player.Name} by {GetSkillMasterTitle(skill)} (Level {questLevel}), Group: {selectedGroup?.GetType().Name}.");
        }

        private static void GiveBaseRewards(PlayerMobile player, int questLevel)
        {
            player.AddToBackpack(new Gold(questLevel * Utility.RandomMinMax(1000, 2000))); // Gold reward
            player.AddToBackpack(new MaxxiaScroll(questLevel * Utility.RandomMinMax(1, 2))); // Guaranteed reward
        }

        private static void GivePowerScrollReward(PlayerMobile player, SkillName skill)
        {
            Skill targetSkill = player.Skills[skill];
            if (targetSkill != null)
            {
                double currentCap = targetSkill.Cap;
                double newCap = Math.Min(currentCap + 10.0, 150.0); // Increase cap by 10, max 150

                if (newCap > currentCap)
                {
                    int scrollValue = (int)newCap;
                    player.AddToBackpack(new PowerScroll(skill, scrollValue));
                }
                else
                {
                    // If the cap is already at max, give a fallback PowerScroll
                    player.AddToBackpack(new PowerScroll(skill, 120));
                }
            }
            else
            {
                // Fallback in case the skill doesn't exist
                Console.WriteLine($"Skill {skill} not found for player {player.Name}.");
                player.AddToBackpack(new PowerScroll(skill, 120));
            }
        }

        public static string GetSkillMasterTitle(SkillName skill)
        {
            return SkillMasterTitles.ContainsKey(skill) ? SkillMasterTitles[skill] : "Unknown Master";
        }
    }
}
