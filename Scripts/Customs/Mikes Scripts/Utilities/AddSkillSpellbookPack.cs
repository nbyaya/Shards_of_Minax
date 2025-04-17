using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
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
using Server.ACC.CSS.Systems.Pastoralicon;
using Server.ACC.CSS.Systems.MartialManual;

namespace Server.Commands
{
    public class AddSkillSpellbookPack
    {
        public static void Initialize()
        {
            CommandSystem.Register("AddSkillSpellbookPack", AccessLevel.Administrator, new CommandEventHandler(AddSkillSpellbookPack_OnCommand));
        }

        [Usage("AddSkillSpellbookPack")]
        [Description("Adds a backpack filled with all skill-based spellbooks to your inventory.")]
        public static void AddSkillSpellbookPack_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || from.Deleted)
                return;

            // Create the backpack
            Backpack pack = new Backpack
            {
                Name = "Skill Spellbook Pack"
            };

            // List of spellbook types to add
            Type[] spellbookTypes =
            {
                typeof(AlchemySpellbook),
                typeof(FishingSpellbook),
                typeof(EvalIntSpellbook),
                typeof(ArcherySpellbook),
                typeof(MagerySpellbook),
                typeof(ArmsLoreSpellbook),
                typeof(AnimalTamingSpellbook),
                typeof(AnimalLoreSpellbook),
                typeof(CarpentrySpellbook),
                typeof(CartographySpellbook),
                typeof(TasteIDSpellbook),
                typeof(CookingSpellbook),
                typeof(DiscordanceSpellbook),
                typeof(FencingSpellbook),
                typeof(FletchingSpellbook),
                typeof(ForensicsSpellbook),
                typeof(WrestlingSpellbook),
                typeof(ParrySpellbook),
                typeof(HealingSpellbook),
                typeof(DetectHiddenSpellbook),
                typeof(ProvocationSpellbook),
                typeof(LockpickingSpellbook),
                typeof(MacingSpellbook),
                typeof(MeditationSpellbook),
                typeof(BeggingSpellbook),
                typeof(MiningSpellbook),
                typeof(ChivalrySpellbook2),
                typeof(StealingSpellbook),
                typeof(InscribeSpellbook),
                typeof(NinjitsuSpellbook),
                typeof(HidingSpellbook),
                typeof(StealthSpellbook),
                typeof(BlacksmithSpellbook),
                typeof(TacticsSpellbook),
                typeof(SwordsSpellbook),
                typeof(TailoringSpellbook),
                typeof(NecromancySpellbook),
                typeof(TrackingSpellbook),
                typeof(RemoveTrapSpellbook),
                typeof(VeterinarySpellbook),
                typeof(MusicianshipSpellbook),
                typeof(CampingSpellbook),
				typeof(PastoraliconBook),
				typeof(MartialManualBook),
                typeof(LumberjackingSpellbook)
            };

            // Add each spellbook to the backpack
            foreach (Type spellbookType in spellbookTypes)
            {
                Item spellbook = (Item)Activator.CreateInstance(spellbookType);
                if (spellbook != null)
                    pack.DropItem(spellbook);
            }

            // Add the backpack to the player's inventory
            from.AddToBackpack(pack);
            from.SendMessage("A backpack filled with skill spellbooks has been added to your inventory.");
        }
    }
}
