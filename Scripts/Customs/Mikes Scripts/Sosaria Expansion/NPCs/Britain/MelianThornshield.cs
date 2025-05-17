using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VenomousWatchQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Venomous Watch"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Melian Thornshield*, the vigilant Ward Keeper of Castle British.\n\n" +
                    "Clad in robes of storm-silver and dusk-blue, her eyes flicker with arcane light, and her fingers instinctively trace glyphs of protection.\n\n" +
                    "“The Vault stirs. One of the old Sentinels—awakened, venom running through its gears. It stalks the containment cells now, waiting for any fool to breach the seals.”\n\n" +
                    "“I felt it. My talismans rattled like a struck bell. The **StingerClassSentinel** is active again, poison-tipped, mindless. I’ve sent too many scouts with antidotes. They don’t return.”\n\n" +
                    "“But you—you might sever its stinger. You might end this, before the toxin spreads to other vaults.”\n\n" +
                    "**Slay the StingerClassSentinel** in Preservation Vault 44. Return with the stinger’s core, or we lose more than relics—we lose the castle’s wards.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I hope you stay far from the vault, and further from its sting.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still watches? The air grows heavy. My wards struggle to hold the lines. Hurry.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve severed the stinger, and I can breathe again.\n\n" +
                       "The wards calm, the talismans rest. And the Vault? Silent. Take this: *WhispersOfChaos*. It will remind you of what stirs when balance fails.";
            }
        }

        public VenomousWatchQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(StingerClassSentinel), "StingerClassSentinel", 1));
            AddReward(new BaseReward(typeof(WhispersOfChaos), 1, "WhispersOfChaos"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Venomous Watch'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MelianThornshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VenomousWatchQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public MelianThornshield()
            : base("the Ward Keeper", "Melian Thornshield")
        {
        }

        public MelianThornshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1152; // Pale, almost luminescent skin tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1157; // Dark violet
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 2407, Name = "Stormwoven Robe" }); // Deep blue-silver hue
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Talisman-Gripped Gloves" }); // Blackened leather
            AddItem(new Sandals() { Hue = 1175, Name = "Ward-Walker Sandals" }); // Midnight blue

            AddItem(new Cloak() { Hue = 2407, Name = "Veil of Warding" });
            AddItem(new LeatherCap() { Hue = 1175, Name = "Thornshield Circlet" });

            AddItem(new SpellWeaversWand() { Hue = 1260, Name = "Antidote-Arrow Wand" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1155;
            backpack.Name = "Runed Satchel";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
