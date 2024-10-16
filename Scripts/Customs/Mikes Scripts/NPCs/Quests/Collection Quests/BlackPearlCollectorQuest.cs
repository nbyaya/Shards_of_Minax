using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BlackPearlCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Black Pearl Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am in dire need of Black Pearls for my research. " +
                       "If you could bring me 50 Black Pearls, I would be eternally grateful and reward you handsomely!";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, I'll be here."; } }

        public override object Uncomplete { get { return "You haven't gathered enough Black Pearls yet. Keep searching!"; } }

        public override object Complete { get { return "You've collected all the Black Pearls I need. Thank you! Here's your reward."; } }

        public BlackPearlCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BlackPearl), "Black Pearls", 50, 0xF7A));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(ChefsGourmetApron), 1, "Black Pearl Apron"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have successfully completed the Black Pearl Collector's Request!");
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

    public class BlackPearlCollector : MondainQuester
    {
        [Constructable]
        public BlackPearlCollector()
            : base("The Mystic", "Black Pearl Collector")
        {
        }

        public BlackPearlCollector(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Female = false;
            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Random hair style
            HairHue = 0; // Black hair
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1157)); // Dark blue robe
            AddItem(new Sandals(0)); // Sandals
            AddItem(new WizardsHat(1157)); // Dark blue hat
            AddItem(new GnarledStaff { Name = "Mystic's Staff", Hue = 1157 }); // Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Bag of Mystical Treasures";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BlackPearlCollectorQuest)
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
