using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class FertileDirtCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Fertile Dirt Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble adventurer! I am Eldara, the Earthshaper. I need your help to gather 50 Fertile Dirt. " +
                       "This rare and valuable soil is crucial for my land restoration efforts. In return for your help, you will receive " +
                       "a reward of gold, a rare Maxxia Scroll, and a unique Earthshaper's Garb that will mark you as a true guardian of nature.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, bring the Fertile Dirt to me."; } }

        public override object Uncomplete { get { return "I still need 50 Fertile Dirt. Please gather them for me."; } }

        public override object Complete { get { return "Excellent work! You have collected the 50 Fertile Dirt I requested. Your effort is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May the earth be forever in your favor!"; } }

        public FertileDirtCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FertileDirt), "Fertile Dirt", 50, 0xF81)); // Assuming Fertile Dirt item ID is 0x0F9F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DancersEnchantedSkirt), 1, "Earthshaper's Garb")); // Assuming Earthshaper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Fertile Dirt Collector quest!");
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

    public class EarthshaperEldara : MondainQuester
    {
        [Constructable]
        public EarthshaperEldara()
            : base("The Earthshaper", "Eldara")
        {
        }

        public EarthshaperEldara(Serial serial)
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
            AddItem(new PlainDress { Hue = Utility.Random(1, 3000), Name = "Eldara's Earthshaper Robe" });
            AddItem(new ThighBoots { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Eldara's Nature Crown" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Eldara's Earth Band" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Eldara's Nature Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Eldara's Satchel of Earth";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FertileDirtCollectorQuest)
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
