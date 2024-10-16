using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SextantPartCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sextant's Secrets"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave adventurer! I am Aelric, the Keeper of the Lost Cartography. Long ago, a mighty explorer lost " +
                       "fragments of an ancient sextant, an artifact of great power and mystery. I need your help to recover 50 Sextant Parts " +
                       "to restore this relic to its former glory. In return for your invaluable assistance, I will grant you a generous reward, " +
                       "including gold, a rare Maxxia Scroll, and a grandly embellished Sextant Keeper's Attire.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Sextant Parts."; } }

        public override object Uncomplete { get { return "I still require 50 Sextant Parts to complete the sextant restoration. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Incredible! You've recovered all 50 Sextant Parts. Your efforts are truly appreciated. Accept these rewards as a token of my gratitude. May the stars guide your path!"; } }

        public SextantPartCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SextantParts), "Sextant Parts", 50, 0x1059)); // Assuming Sextant Part item ID is 0xF9F
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ProwlersGloves), 1, "Sextant Keeper's Attire")); // Assuming Sextant Keeper's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sextant's Secrets quest!");
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

    public class SextantKeeperAelric : MondainQuester
    {
        [Constructable]
        public SextantKeeperAelric()
            : base("The Sextant Keeper", "Aelric")
        {
        }

        public SextantKeeperAelric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Aelric's Sextant Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Aelric's Cartographer's Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Aelric's Navigator's Gloves" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Aelric's Enchanted Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aelric's Cartography Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SextantPartCollectorQuest)
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
