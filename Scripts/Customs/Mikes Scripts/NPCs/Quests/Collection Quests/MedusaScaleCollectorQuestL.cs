using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MedusaScaleCollectorQuestL : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Medusa's Lament"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Seraphius, the Guardian of Ancient Secrets. Long ago, Medusa, the serpent queen, " +
                       "was vanquished, but her essence still lingers in the form of her scales. I seek to collect 50 Medusa Light Scales. " +
                       "These scales hold ancient powers and are crucial for a powerful ritual that may bring peace to our realm. " +
                       "In return for your valiant efforts, I shall bestow upon you gold, a rare Maxxia Scroll, and an enchanted Seraphius's Mantle.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Medusa Light Scales."; } }

        public override object Uncomplete { get { return "I still require 50 Medusa Light Scales. Please bring them to me so that I may complete the ritual."; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 Medusa Light Scales. Your bravery and dedication are commendable. " +
                       "Please accept these rewards as a token of my gratitude. May the blessings of the ancients be upon you!"; } }

        public MedusaScaleCollectorQuestL() : base()
        {
            AddObjective(new ObtainObjective(typeof(MedusaLightScales), "Medusa Light Scales", 50, 0xF3F)); // Assuming Medusa Light Scale item ID is 0xF3F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PiledriversPauldrons), 1, "Seraphius's Mantle")); // Assuming Seraphius's Mantle is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed The Medusa's Lament quest!");
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

    public class Seraphius : MondainQuester
    {
        [Constructable]
        public Seraphius()
            : base("The Guardian of Ancient Secrets", "Seraphius")
        {
        }

        public Seraphius(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Seraphius's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Seraphius's Mystical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Seraphius's Arcane Gloves" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Seraphius's Ancient Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphius's Artifact Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MedusaScaleCollectorQuestL)
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
}
