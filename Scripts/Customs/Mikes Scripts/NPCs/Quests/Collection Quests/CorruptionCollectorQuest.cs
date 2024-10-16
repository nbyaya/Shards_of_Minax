using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CorruptionCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Corruption Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler. I am Draven the Warden, custodian of ancient secrets and protector of the realms. " +
                       "I require your aid in collecting 50 Corruption, the dark remnants of a long-forgotten evil. " +
                       "These fragments are essential to my efforts in sealing away a looming menace. In return for your efforts, " +
                       "I shall grant you gold, a rare Maxxia Scroll, and a Warden's Cloak imbued with mystical properties.";
            }
        }

        public override object Refuse { get { return "Very well. Should you choose to aid me, return with the Corruption."; } }

        public override object Uncomplete { get { return "I still require 50 Corruption. Please bring them to me to aid in our cause!"; } }

        public override object Complete { get { return "Well done! You have gathered the 50 Corruption I sought. Your bravery aids us in our fight against the darkness. " +
                       "Accept these rewards as a token of my appreciation. May your path be ever illuminated!"; } }

        public CorruptionCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Corruption), "Corruption", 50, 0x3184)); // Assuming Corruption item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(JazzMusiciansMuffler), "Warden's Muffler")); // Assuming Warden's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Corruption Collector quest!");
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

    public class DravenTheWarden : MondainQuester
    {
        [Constructable]
        public DravenTheWarden()
            : base("The Warden", "Draven")
        {
        }

        public DravenTheWarden(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Draven's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Draven's Mystical Cap" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Draven's Protective Gloves" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Draven's Shadowed Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CorruptionCollectorQuest)
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
