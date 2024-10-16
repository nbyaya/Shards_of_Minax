using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FaeryDustCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Faery Dust Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Sylvaria, the Faery Sage. I require your aid to collect 50 Faery Dust. " +
                       "These magical particles are essential for my research and spellcasting. As a token of my gratitude, you will be " +
                       "rewarded with gold, a rare Maxxia Scroll, and a unique Faery Sage's Boots that will make you look like a true master of magic.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Faery Dust."; } }

        public override object Uncomplete { get { return "I still need 50 Faery Dust. Please bring them to me so I can continue my research!"; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 Faery Dust I needed. Your assistance is greatly appreciated. " +
                       "Please accept these rewards as a symbol of my gratitude. May your magic be ever powerful!"; } }

        public FaeryDustCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FaeryDust), "Faery Dust", 50, 0x5745)); // Assuming Faery Dust item ID is 0xF8E
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GoGoBootsOfAgility), 1, "Faery Sage's Boots")); // Assuming Faery Sage's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Faery Dust Collector quest!");
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

    public class FaerySageSylvaria : MondainQuester
    {
        [Constructable]
        public FaerySageSylvaria()
            : base("The Faery Sage", "Sylvaria")
        {
        }

        public FaerySageSylvaria(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress { Hue = Utility.Random(1, 3000), Name = "Sylvaria's Faery Robe" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Sylvaria's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Sylvaria's Magic Ring" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Sylvaria's Mystical Necklace" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Sylvaria's Spellbook Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FaeryDustCollectorQuest)
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
