using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CakeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Cake Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Beatrix, the Enchanted Baker. My grand cake collection has mysteriously vanished, " +
                       "and I need your help to recover 50 Cakes. These cakes are not ordinary—they hold the secrets to my legendary recipes! " +
                       "In exchange for your efforts, I will reward you with gold, a rare Maxxia Scroll, and a special attire that will make you " +
                       "the envy of every baker in the land.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the Cakes."; } }

        public override object Uncomplete { get { return "I still need 50 Cakes to complete my collection. Please bring them to me!"; } }

        public override object Complete { get { return "You've done it! You have gathered the 50 Cakes I needed. Your help is truly appreciated. " +
                       "Accept these rewards as a token of my gratitude. May your adventures be as sweet as these cakes!"; } }

        public CakeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Cake), "Cakes", 50, 0x9E9)); // Assuming Cake item ID is 0x9C0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BowmastersRingmailArms), 1, "Beatrix's Baker Arms")); // Assuming Baker's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed The Great Cake Hunt quest!");
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

    public class EnchantedBakerBeatrix : MondainQuester
    {
        [Constructable]
        public EnchantedBakerBeatrix()
            : base("The Enchanted Baker", "Beatrix")
        {
        }

        public EnchantedBakerBeatrix(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Beatrix's Enchanted Skirt" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Beatrix's Baker Apron" });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Beatrix's Floral Crown" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Beatrix's Baking Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Beatrix's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CakeCollectorQuest)
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
