using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AshesOfTheAncientQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ashes of the Ancient"; } }

        public override object Description
        {
            get
            {
                return
                    "*Captain Roderick Flamebeard*, once a scourge of dragons, now a ghost of glory past.\n\n" +
                    "His armor bears scorch marks, his eyes heavy with unspoken burdens.\n\n" +
                    "“Aye, I've slain dragons with steel and fire. But this one? This cursed **Dracolich**? It mocks everything I fought for.”\n\n" +
                    "“It stalks the grand hall of the Malidor Witches Academy... laughing with dead lungs. I saw it once, and fled like a child.”\n\n" +
                    "“I keep this.”\n\n" +
                    "*He shows a vial—gray dust that swirls unnaturally.*\n\n" +
                    "“A piece of its remains, taken from an old battlefield. I need more. I need vengeance. **Slay the Dracolich**, and bring its death back to me.”\n\n" +
                    "“Let me rest knowing the wyrm won’t haunt my name.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your courage not falter as mine once did. But beware—it will come for you, as it did for me.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it lives? The Dracolich breathes mockery into the wind... and I feel its chill each night.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You... you’ve done it? I can feel it—the air is lighter, the screams quieter.\n\n" +
                       "*He grips the vial, now glowing faintly.*\n\n" +
                       "“Its ashes... will rest with me now. Thank you, warrior. May your meals be warm, and your nights free of dragons.”";
            }
        }

        public AshesOfTheAncientQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Dracolich), "Dracolich", 1));
            AddReward(new BaseReward(typeof(DinerDelightChest), 1, "DinerDelightChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ashes of the Ancient'!");
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

    public class CaptainRoderickFlamebeard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AshesOfTheAncientQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMaceWeapon());
        }

        [Constructable]
        public CaptainRoderickFlamebeard()
            : base("the Retired Dragon-Slayer", "Captain Roderick Flamebeard")
        {
        }

        public CaptainRoderickFlamebeard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 60);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = 1023; // Weathered skin tone
            HairItemID = 0x2044; // Long Hair
            HairHue = 1359; // Fiery red-orange
            FacialHairItemID = 0x204B; // Full Beard
            FacialHairHue = 1359; // Matching fiery hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1157, Name = "Ember-Scarred Chestplate" }); // Dark reddish
            AddItem(new PlateLegs() { Hue = 1157, Name = "Ashen Greaves" });
            AddItem(new StuddedGloves() { Hue = 2101, Name = "Charred Gauntlets" });
            AddItem(new WingedHelm() { Hue = 1175, Name = "Dragonfang Helm" }); // Deep red with black highlights
            AddItem(new Cloak() { Hue = 1171, Name = "Smoldering Cloak" }); // Smoky black
            AddItem(new ThighBoots() { Hue = 1175, Name = "Dragonhide Boots" });

            AddItem(new WarHammer() { Hue = 1161, Name = "Flamebeard's Wrath" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Dragon-Hunter's Satchel";
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
