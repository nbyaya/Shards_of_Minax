using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ValoriteCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Valorite Ingot Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! Our smiths are in dire need of Valorite ingots for their work. " +
                       "If you could bring me 50 Valorite ingots, I will reward you with a fine prize and my eternal gratitude.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, come back and let me know."; } }

        public override object Uncomplete { get { return "You haven't collected enough Valorite ingots yet. Please bring me 50 ingots."; } }

        public override object Complete { get { return "Excellent work! Here is your reward for collecting the Valorite ingots."; } }

        public ValoriteCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ValoriteIngot), "Valorite Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(DiscoDivaBoots), 1, "Valorite Boots"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have successfully completed the Valorite Ingot Request!");
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

    public class ValoriteCollectorBoris : MondainQuester
    {
        [Constructable]
        public ValoriteCollectorBoris()
            : base("The Master Smith", "Boris the Valorite Collector")
        {
        }

        public ValoriteCollectorBoris(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body
            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2042; // Long hair style
            HairHue = 0x501; // Dark brown hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new BodySash(1153)); // Noble's sash
            AddItem(new FancyShirt(1153)); // Noble's shirt
            AddItem(new LongPants(1153)); // Noble's pants
            AddItem(new Boots(1153)); // Noble's boots
            AddItem(new WideBrimHat(1153)); // Noble's hat
            AddItem(new SmithHammer { Name = "Boris's Smithing Hammer", Hue = 0 }); // Custom item

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Smith's Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ValoriteCollectorQuest)
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
