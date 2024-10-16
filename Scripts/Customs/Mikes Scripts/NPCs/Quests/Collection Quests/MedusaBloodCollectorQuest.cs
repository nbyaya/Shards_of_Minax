using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MedusaBloodCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Medusa's Blood Collector"; } }

        public override object Description
        {
            get
            {
                return "Ah, welcome, brave adventurer! I am Aleron, the Keeper of the Serpent's Secrets. The blood of Medusa is vital " +
                       "for an ancient ritual that will strengthen our defenses against the encroaching forces of darkness. I require " +
                       "50 vials of Medusa's Blood to complete this powerful spell. In return for your valiant effort, you shall receive " +
                       "a generous reward, including gold, a rare Maxxia Scroll, and a unique Boots imbued with the essence of the serpent.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, bring me the Medusa's Blood when you're ready."; } }

        public override object Uncomplete { get { return "I still need 50 vials of Medusa's Blood. Please return once you have gathered them all."; } }

        public override object Complete { get { return "You've done it! You’ve gathered the 50 vials of Medusa's Blood I required. Your contribution will greatly aid our cause. " +
                       "Please accept these rewards as a token of my appreciation. May the serpent's protection guide you on your path!"; } }

        public MedusaBloodCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MedusaBlood), "Medusa's Blood", 50, 0x2DB6)); // Assuming Medusa's Blood item ID is 0xF7B
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ExplorersBoots), 1, "Serpent Keeper's Boots")); // Assuming Serpent Keeper's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Medusa's Blood Collector quest!");
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

    public class SerpentKeeperAleron : MondainQuester
    {
        [Constructable]
        public SerpentKeeperAleron()
            : base("The Keeper of the Serpent's Secrets", "Aleron")
        {
        }

        public SerpentKeeperAleron(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Aleron's Serpent Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Aleron's Serpent Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Aleron's Scroll of Secrets" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Aleron's Secret Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MedusaBloodCollectorQuest)
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
