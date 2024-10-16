using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class StarSapphireCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Star Sapphire Seeker"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Seraphine, Keeper of Celestial Wonders. I seek your aid in gathering 50 Star Sapphires, " +
                       "rare gems that hold the essence of the night sky itself. These sapphires are vital for a grand celestial ritual I am " +
                       "preparing. Your reward will be gold, a mystical Maxxia Scroll, and a celestial garb that shines with the brilliance of the stars.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the Star Sapphires."; } }

        public override object Uncomplete { get { return "The celestial ritual awaits! I still need 50 Star Sapphires. Please bring them to me when you can."; } }

        public override object Complete { get { return "Splendid work! You have collected the 50 Star Sapphires I needed. Your contribution is invaluable. " +
                       "As a token of my appreciation, accept these rewards. May the stars guide your path!"; } }

        public StarSapphireCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(StarSapphire), "Star Sapphires", 50, 0xF21)); // Assuming Star Sapphire item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(AnvilsGuardLegs), 1, "Celestial Garb")); // Assuming Celestial Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Star Sapphire Seeker quest!");
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

    public class CelestialKeeperSeraphine : MondainQuester
    {
        [Constructable]
        public CelestialKeeperSeraphine()
            : base("The Celestial Keeper", "Seraphine")
        {
        }

        public CelestialKeeperSeraphine(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress { Hue = Utility.Random(1, 3000), Name = "Seraphine's Celestial Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Seraphine's Starry Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Seraphine's Celestial Ring" });
            AddItem(new Crossbow { Hue = Utility.Random(1, 3000), Name = "Seraphine's Star Wand" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphine's Celestial Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(StarSapphireCollectorQuest)
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
