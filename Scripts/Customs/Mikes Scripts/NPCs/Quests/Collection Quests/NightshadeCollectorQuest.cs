using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class NightshadeCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Nightshade Harvest"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings brave adventurer! I am Mortis, the Shadow Alchemist. " +
                       "I require your aid to collect 500 Nightshade. These potent herbs are essential for my dark elixirs and enchantments. " +
                       "In return for your efforts, I shall reward you with gold, a rare Maxxia Scroll, and the enigmatic Nightshade Bandana, " +
                       "woven with shadows and secrets.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Nightshade."; } }

        public override object Uncomplete { get { return "I still require 500 Nightshade. Bring them to me so I can continue my work!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 500 Nightshade I needed. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May the shadows favor you on your journey!"; } }

        public NightshadeCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Nightshade), "Nightshade", 500, 0xF88)); // Assuming Nightshade item ID is 0xF5F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(OreSeekersBandana), 1, "Nightshade Bandana")); // Assuming Nightshade Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Nightshade Harvest quest!");
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

    public class ShadowAlchemistMortis : MondainQuester
    {
        [Constructable]
        public ShadowAlchemistMortis()
            : base("The Shadow Alchemist", "Mortis")
        {
        }

        public ShadowAlchemistMortis(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Mortis' Shadow Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Mortis' Enigmatic Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Mortis' Shadow Gloves" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Mortis' Mystical Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Mortis' Alchemist Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(NightshadeCollectorQuest)
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
