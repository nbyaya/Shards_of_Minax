using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ToxicVenomCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Toxic Venom Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Zara the Venomous. The world teems with danger, and " +
                       "I require your aid in gathering 50 Toxic Venom Sacs. These vile substances are essential for " +
                       "my research into antidotes and protective enchantments. In return for your valuable assistance, " +
                       "I shall bestow upon you gold, a rare Maxxia Scroll, and an outfit imbued with the essence of venom itself.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, bring the Toxic Venom Sacs to me."; } }

        public override object Uncomplete { get { return "I still need 50 Toxic Venom Sacs. Your help is crucial for my research!"; } }

        public override object Complete { get { return "Excellent! You have brought me the 50 Toxic Venom Sacs I required. Your efforts are deeply appreciated. " +
                       "As a token of my gratitude, accept these rewards and wear them with pride. May you be ever protected from the perils of the wild!"; } }

        public ToxicVenomCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ToxicVenomSac), "Toxic Venom Sacs", 50, 0x4005)); // Assuming Toxic Venom Sac item ID is 0xF0A
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GhostlyShroud), 1, "Venomous Outfit")); // Assuming Venomous Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Toxic Venom Collector quest!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ZaraTheVenomous : MondainQuester
    {
        [Constructable]
        public ZaraTheVenomous()
            : base("The Venomous", "Zara")
        {
        }

        public ZaraTheVenomous(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress { Hue = Utility.Random(1, 3000), Name = "Zara's Toxic Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Zara's Venomous Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Zara's Venomous Gloves" });
            AddItem(new Skirt { Hue = Utility.Random(1, 3000), Name = "Zara's Venomous Skirt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Zara's Venomous Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ToxicVenomCollectorQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VenomousOutfit : Item
    {
        [Constructable]
        public VenomousOutfit() : base(0x1F03) // Use an existing base item ID
        {
            Hue = Utility.Random(1, 3000); // Unique hue
            Name = "Venomous Outfit";
        }

        public VenomousOutfit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
