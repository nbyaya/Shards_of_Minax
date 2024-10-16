using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GraveDustCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Cursed Relic"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Malakor, the Keeper of Lost Souls. My tome of ancient rites requires 50 pieces of GraveDust, " +
                       "a vital component for my ritual to banish the cursed spirits that haunt these lands. In return for your assistance, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a unique relic that has the power to aid you in your journeys.";
            }
        }

        public override object Refuse { get { return "I understand. Should you reconsider, return to me with the GraveDust."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of GraveDust. Gather them for me, and I shall reward you handsomely!"; } }

        public override object Complete { get { return "Your courage and diligence have proven invaluable! You have brought me the 50 pieces of GraveDust I needed. " +
                       "As a token of my gratitude, please accept these rewards. May the spirits of the dead find peace, and may you find fortune in your travels!"; } }

        public GraveDustCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GraveDust), "GraveDust", 50, 0xF8F)); // Assuming GraveDust item ID is 0x1F6
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ArchonsMysticRobe), 1, "Cursed Relic")); // Assuming Cursed Relic is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Cursed Relic quest!");
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

    public class KeeperMalakor : MondainQuester
    {
        [Constructable]
        public KeeperMalakor()
            : base("The Keeper of Lost Souls", "Malakor")
        {
        }

        public KeeperMalakor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Malakor's Enchanted Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Malakor's Ritual Cap" });
            AddItem(new Spellbook { Hue = Utility.Random(1, 3000), Name = "Malakor's Arcane Tome" }); // Assuming this is a custom item
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Malakor's Soul Binding Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Malakor's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GraveDustCollectorQuest)
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
