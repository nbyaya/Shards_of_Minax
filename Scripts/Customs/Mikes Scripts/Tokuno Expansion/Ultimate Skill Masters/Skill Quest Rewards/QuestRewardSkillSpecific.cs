using Server.ACC.CSS.Systems.AlchemyMagic;
using Server.ACC.CSS.Systems.AnimalLoreMagic;
using Server.ACC.CSS.Systems.AnimalTamingMagic;
using Server.ACC.CSS.Systems.ArcheryMagic;
using Server.ACC.CSS.Systems.ArmsLoreMagic;
using Server.ACC.CSS.Systems.BeggingMagic;
using Server.ACC.CSS.Systems.BlacksmithMagic;
using Server.ACC.CSS.Systems.CampingMagic;
using Server.ACC.CSS.Systems.CarpentryMagic;
using Server.ACC.CSS.Systems.CartographyMagic;
using Server.ACC.CSS.Systems.ChivalryMagic;
using Server.ACC.CSS.Systems.CookingMagic;
using Server.ACC.CSS.Systems.DetectHiddenMagic;
using Server.ACC.CSS.Systems.DiscordanceMagic;
using Server.ACC.CSS.Systems.EvalIntMagic;
using Server.ACC.CSS.Systems.FencingMagic;
using Server.ACC.CSS.Systems.FishingMagic;
using Server.ACC.CSS.Systems.FletchingMagic;
using Server.ACC.CSS.Systems.ForensicsMagic;
using Server.ACC.CSS.Systems.HealingMagic;
using Server.ACC.CSS.Systems.HidingMagic;
using Server.ACC.CSS.Systems.InscribeMagic;
using Server.ACC.CSS.Systems.LockpickingMagic;
using Server.ACC.CSS.Systems.LumberjackingMagic;
using Server.ACC.CSS.Systems.MacingMagic;
using Server.ACC.CSS.Systems.MageryMagic;
using Server.ACC.CSS.Systems.MeditationMagic;
using Server.ACC.CSS.Systems.MiningMagic;
using Server.ACC.CSS.Systems.MusicianshipMagic;
using Server.ACC.CSS.Systems.NecromancyMagic;
using Server.ACC.CSS.Systems.NinjitsuMagic;
using Server.ACC.CSS.Systems.ParryMagic;
using Server.ACC.CSS.Systems.ProvocationMagic;
using Server.ACC.CSS.Systems.RemoveTrapMagic;
using Server.ACC.CSS.Systems.StealingMagic;
using Server.ACC.CSS.Systems.StealthMagic;
using Server.ACC.CSS.Systems.SwordsMagic;
using Server.ACC.CSS.Systems.TacticsMagic;
using Server.ACC.CSS.Systems.TailoringMagic;
using Server.ACC.CSS.Systems.TasteIDMagic;
using Server.ACC.CSS.Systems.TrackingMagic;
using Server.ACC.CSS.Systems.VeterinaryMagic;
using Server.ACC.CSS.Systems.WrestlingMagic;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters.Skill_Quest_Rewards
{
    public class QuestRewardSkillSpecific : IRewardGroup
    {
        public int Weight => 50; // Set the weight for this group

        public Type[] GetRewards(PlayerMobile player, SkillName? skill)
        {
            return SkillSpecificRewards.ContainsKey(skill.Value)
                ? SkillSpecificRewards[skill.Value]
                : Array.Empty<Type>();
        }

        private static readonly Dictionary<SkillName, Type[]> SkillSpecificRewards = new Dictionary<SkillName, Type[]>
        {
            { SkillName.Alchemy, new[] { typeof(AlchemySpellbook) } },
            { SkillName.AnimalLore, new[] { typeof(AnimalLoreSpellbook) } },
            { SkillName.AnimalTaming, new[] { typeof(AnimalTamingSpellbook) } },
            { SkillName.Archery, new[] { typeof(ArcherySpellbook) } },
            { SkillName.ArmsLore, new[] { typeof(ArmsLoreSpellbook) } },
            { SkillName.Begging, new[] { typeof(BeggingSpellbook) } },
            { SkillName.Blacksmith, new[] { typeof(BlacksmithSpellbook) } },
            { SkillName.Bushido, new[] { typeof(BookOfBushido) } },
            { SkillName.Camping, new[] { typeof(CampingSpellbook) } },
            { SkillName.Carpentry, new[] { typeof(CarpentrySpellbook) } },
            { SkillName.Cartography, new[] { typeof(CartographySpellbook) } },
            { SkillName.Chivalry, new[] { typeof(ChivalrySpellbook2) } },
            { SkillName.Cooking, new[] { typeof(CookingSpellbook) } },
            { SkillName.DetectHidden, new[] { typeof(DetectHiddenSpellbook) } },
            { SkillName.Discordance, new[] { typeof(DiscordanceSpellbook) } },
            { SkillName.EvalInt, new[] { typeof(EvalIntSpellbook) } },
            { SkillName.Fencing, new[] { typeof(FencingSpellbook) } },
            { SkillName.Fishing, new[] { typeof(FishingSpellbook) } },
            { SkillName.Fletching, new[] { typeof(FletchingSpellbook) } },
            { SkillName.Forensics, new[] { typeof(ForensicsSpellbook) } },
            { SkillName.Healing, new[] { typeof(HealingSpellbook) } },
            { SkillName.Hiding, new[] { typeof(HidingSpellbook) } },
            { SkillName.Inscribe, new[] { typeof(InscribeSpellbook) } },
            { SkillName.Lockpicking, new[] { typeof(LockpickingSpellbook) } },
            { SkillName.Lumberjacking, new[] { typeof(LumberjackingSpellbook) } },
            { SkillName.Macing, new[] { typeof(MacingSpellbook) } },
            { SkillName.Magery, new[] { typeof(MagerySpellbook) } },
            { SkillName.Meditation, new[] { typeof(MeditationSpellbook) } },
            { SkillName.Mining, new[] { typeof(MiningSpellbook) } },
            { SkillName.Musicianship, new[] { typeof(MusicianshipSpellbook) } },
            { SkillName.Necromancy, new[] { typeof(NecromancySpellbook) } },
            { SkillName.Ninjitsu, new[] { typeof(NinjitsuSpellbook) } },
            { SkillName.Parry, new[] { typeof(ParrySpellbook) } },
            { SkillName.Provocation, new[] { typeof(ProvocationSpellbook) } },
            { SkillName.RemoveTrap, new[] { typeof(RemoveTrapSpellbook) } },
            { SkillName.Stealing, new[] { typeof(StealingSpellbook) } },
            { SkillName.Stealth, new[] { typeof(StealthSpellbook) } },
            { SkillName.Swords, new[] { typeof(SwordsSpellbook) } },
            { SkillName.Tactics, new[] { typeof(TacticsSpellbook) } },
            { SkillName.Tailoring, new[] { typeof(TailoringSpellbook) } },
            { SkillName.TasteID, new[] { typeof(TasteIDSpellbook) } },
            { SkillName.Tracking, new[] { typeof(TrackingSpellbook) } },
            { SkillName.Veterinary, new[] { typeof(VeterinarySpellbook) } },
            { SkillName.Wrestling, new[] { typeof(WrestlingSpellbook) } },
        };
    }
}
