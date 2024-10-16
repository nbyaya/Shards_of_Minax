using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CandyCaneCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Candy Cane Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave wanderer! I am Frostina, the Frostborn Enchantress. I seek your aid in gathering " +
                       "50 Candy Canes. These sweet treats are not just for holiday cheer but hold mystical powers in their sugary " +
                       "essence. They are essential for crafting the legendary Frostborn Amulet, which I am currently working on. " +
                       "In return for your valuable assistance, you shall receive gold, a rare Maxxia Scroll, and a Frostborn Enchantress's Gown.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, return to me with the Candy Canes."; } }

        public override object Uncomplete { get { return "I still need 50 Candy Canes to complete my mystical amulet. Please bring them to me!"; } }

        public override object Complete { get { return "Splendid work! You have gathered the 50 Candy Canes I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May the frosty winds guide your path!"; } }

        public CandyCaneCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CandyCane), "Candy Canes", 50, 0x2bdd)); // Assuming Candy Cane item ID is 0xC4F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CourtesansGracefulKimono), 1, "Frostborn Enchantress's Gown")); // Assuming Frostborn Enchantress's Gown is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Candy Cane Conundrum quest!");
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

    public class FrostbornEnchantress : MondainQuester
    {
        [Constructable]
        public FrostbornEnchantress()
            : base("The Frostborn Enchantress", "Frostina")
        {
        }

        public FrostbornEnchantress(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Frostborn Enchantress's Gown" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Frostborn's Mystical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Frostborn's Enchanted Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Frostborn's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CandyCaneCollectorQuest)
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
