using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class UndyingHorrorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Undying Horror"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings brave adventurer! I am Morgath the Unseen, a seeker of ancient and forbidden knowledge. " +
                       "A vile curse has brought forth grotesque creatures known as Undying Horrors. I need you to collect 50 pieces of " +
                       "Undying Flesh from these abominations. They are crucial for a ritual that could end this plague. In return, " +
                       "I shall reward you with gold, a rare Maxxia Scroll, and a set of unique Cloak that bears the mark of the Unseen.";
            }
        }

        public override object Refuse { get { return "Very well, should you reconsider, return with the Undying Flesh."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of Undying Flesh. Please bring them to me to aid in my ritual!"; } }

        public override object Complete { get { return "Excellent! You have gathered the 50 pieces of Undying Flesh. Your bravery and commitment have been crucial " +
                       "in halting the spread of the curse. Accept these rewards as a token of my gratitude. May the shadows fear you!"; } }

        public UndyingHorrorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(UndyingFlesh), "Undying Flesh", 50, 0x5731)); // Assuming Undying Flesh item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(NaturalistsCloak), 1, "Morgath's Cloak")); // Assuming Morgath's Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Undying Horror quest!");
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

    public class MorgathTheUnseen : MondainQuester
    {
        [Constructable]
        public MorgathTheUnseen()
            : base("Morgath the Unseen", "Morgath")
        {
        }

        public MorgathTheUnseen(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Morgath's Ritual Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Morgath's Mystical Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Morgath's Enchanted Gloves" });
            AddItem(new SkullCap { Hue = Utility.Random(1, 3000), Name = "Morgath's Arcane Skullcap" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Morgath's Grimoire Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(UndyingHorrorQuest)
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
