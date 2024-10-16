using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SeedRenewalCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Seed Renewal Mission"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Veridion, the Herbal Sage. I seek your assistance in gathering 50 Seed of Renewals. " +
                       "These rare seeds possess extraordinary properties that can restore the balance of nature itself. " +
                       "In exchange for your invaluable help, I will reward you with gold, a precious Maxxia Scroll, and an exclusive Herbalist's Attire that embodies the essence of nature.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the SeedRenewals."; } }

        public override object Uncomplete { get { return "I still need 50 SeedRenewals. Please bring them to me to assist in my work!"; } }

        public override object Complete { get { return "Excellent work! You have gathered the 50 SeedRenewals I required. Your dedication is truly commendable. " +
                       "Please accept these rewards as a token of my gratitude. May the seeds of wisdom continue to grow in your journey!"; } }

        public SeedRenewalCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SeedOfRenewal), "SeedRenewals", 50, 0x5736)); // Assuming SeedRenewal item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ForestersTunic), 1, "Herbalist's Attire")); // Assuming Herbalist's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Seed Renewal Mission!");
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

    public class HerbalistVeridion : MondainQuester
    {
        [Constructable]
        public HerbalistVeridion()
            : base("The Herbal Sage", "Veridion")
        {
        }

        public HerbalistVeridion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Veridion's Herbal Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WideBrimHat { Hue = Utility.Random(1, 3000), Name = "Veridion's Sage Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Veridion's Herb Cloak" }); // Assuming this is a custom item
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SeedRenewalCollectorQuest)
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
