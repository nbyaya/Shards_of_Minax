using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MeteoriteCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Celestial Meteorite Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Valtor, the Celestial Sage. The stars have foretold a cosmic event of great importance, " +
                       "but to harness its power, I require 50 Polished Meteorites. These celestial stones are imbued with the essence of the heavens, " +
                       "and their collection will aid me in a ritual to restore balance to our world. In gratitude, I shall reward you with gold, " +
                       "a rare Maxxia Scroll, and the Celestial Sage's Regalia, a garment touched by the stars themselves.";
            }
        }

        public override object Refuse { get { return "I see. Should you decide to assist, return to me with the Polished Meteorites."; } }

        public override object Uncomplete { get { return "I still require 50 Polished Meteorites. Please bring them to me so we can perform the celestial ritual!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Polished Meteorites I needed. Your efforts are truly appreciated. " +
                       "Accept these rewards as a token of my gratitude. May the stars guide you on your journey!"; } }

        public MeteoriteCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PolishedMeteorite), "Polished Meteorites", 50, 41423)); // Assuming Polished Meteorite item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(EmoSceneHairpin), 1, "Celestial Sage's Regalia")); // Assuming Celestial Sage's Regalia is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Celestial Meteorite Collector quest!");
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

    public class CelestialSageValtor : MondainQuester
    {
        [Constructable]
        public CelestialSageValtor()
            : base("The Celestial Sage", "Valtor")
        {
        }

        public CelestialSageValtor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Valtor's Celestial Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new ClothNinjaHood { Hue = Utility.Random(1, 3000), Name = "Valtor's Starry Hood" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Valtor's Cosmic Bracelet" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Valtor's Astral Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Valtor's Starry Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MeteoriteCollectorQuest)
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
