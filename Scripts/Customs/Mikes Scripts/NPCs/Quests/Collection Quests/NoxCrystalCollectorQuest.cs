using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class NoxCrystalCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Nox Crystal Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Maelis, an eccentric alchemist of great renown. I require your assistance to collect 500 NoxCrystals. " +
                       "These crystals are vital for my experimental concoctions that could revolutionize alchemy as we know it. Your aid will be handsomely rewarded with gold, " +
                       "a rare Maxxia Scroll, and a one-of-a-kind Alchemist's Ensemble, imbued with magical properties and adorned with hues that defy imagination.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the NoxCrystals I seek."; } }

        public override object Uncomplete { get { return "I am still in need of 500 NoxCrystals. Please bring them to me to aid in my experiments!"; } }

        public override object Complete { get { return "Fantastic work! You have gathered the 500 NoxCrystals I required. Your contribution is invaluable. " +
                       "As a token of my appreciation, please accept these rewards. May your path be ever illuminated by knowledge!"; } }

        public NoxCrystalCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(NoxCrystal), "NoxCrystals", 500, 0xF8E)); // Assuming NoxCrystal item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NinjasStealthyTabi), 1, "Alchemist's Ensemble")); // Assuming Alchemist's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Nox Crystal Conundrum quest!");
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

    public class MaelisTheEccentric : MondainQuester
    {
        [Constructable]
        public MaelisTheEccentric()
            : base("The Eccentric Alchemist", "Maelis")
        {
        }

        public MaelisTheEccentric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new JesterSuit { Hue = Utility.Random(1, 3000), Name = "Maelis's Alchemical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Maelis's Eccentric Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Maelis's Alchemist Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Maelis's Enchanted Kilt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Maelis's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(NoxCrystalCollectorQuest)
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
