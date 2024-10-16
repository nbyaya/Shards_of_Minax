using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class IronIngotQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Blacksmith's Request"; } }

        public override object Description
        {
            get
            {
                return "Hello, traveler! I am in need of some iron ingots to forge new tools. " +
                       "Would you be interested in helping me gather some?";
            }
        }

        public override object Refuse { get { return "Very well. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough iron ingots yet. Keep at it!"; } }

        public override object Complete { get { return "Thank you! Here is your reward for helping with the iron ingots."; } }

        public IronIngotQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(IronIngot), "Iron Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(TruckersIconicCap), 1, "Bob's Forging Cap"));
            AddReward(new BaseReward(typeof(AnvilEastDeed), 1, "Anvil Deed"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Blacksmith's Request!");
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

    public class BlacksmithBob : MondainQuester
    {

        [Constructable]
        public BlacksmithBob()
            : base("The Master Smith", "Blacksmith Bob")
        {
        }

        public BlacksmithBob(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 400; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 1150; // Random hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(88));
            AddItem(new LongPants(88));
            AddItem(new SmithHammer { Name = "Bob's Forging Hammer" });
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bob's Smithing Tools";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(IronIngotQuest)
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
