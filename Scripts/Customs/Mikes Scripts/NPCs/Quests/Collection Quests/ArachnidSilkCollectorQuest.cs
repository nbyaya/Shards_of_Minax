using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ArachnidSilkCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Arachnid Silk Collector"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Orenna, the Weaver of the Web. My home, the ancient Spider's Den, has been deprived of its magical silks. " +
                       "The SpidersSilk you gather will help me restore the magical wards that protect our realm from dark forces. If you bring me 500 SpidersSilk, " +
                       "I shall reward you with gold, a rare Maxxia Scroll, and a mystical Arachnid Weaver's Kasa imbued with the essence of the spiders' magic.";
            }
        }

        public override object Refuse { get { return "If you change your mind, come back with the SpidersSilk I need."; } }

        public override object Uncomplete { get { return "I am still waiting for 500 SpidersSilk. Please hurry; our realm's protection is at risk!"; } }

        public override object Complete { get { return "Fantastic work! You have collected the 500 SpidersSilk needed to restore the wards. Your bravery and dedication are deeply appreciated. " +
                       "As a token of my gratitude, please accept these rewards. May the spiders' magic guide your path!"; } }

        public ArachnidSilkCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SpidersSilk), "SpidersSilk", 500, 0xF8D)); // Assuming SpidersSilk item ID is 0x1D1
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GeishasGracefulKasa), 1, "Arachnid Weaver's Kasa")); // Assuming Arachnid Weaver's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Arachnid Silk Collector quest!");
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

    public class ArachnidWeaverOrenna : MondainQuester
    {
        [Constructable]
        public ArachnidWeaverOrenna()
            : base("The Arachnid Weaver", "Orenna")
        {
        }

        public ArachnidWeaverOrenna(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Orenna's Arachnid Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Orenna's Enchanted Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Orenna's Tome of Spiders" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Orenna's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ArachnidSilkCollectorQuest)
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
