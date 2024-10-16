using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ArcanicRuneCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Arcanic Rune Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, intrepid explorer! I am Eldara, the enigmatic sorceress of the arcane arts. " +
                       "I have a task that requires your exceptional talents. I need you to gather 50 Arcanic Rune Stones for " +
                       "a powerful ritual I am preparing. These stones hold immense magical power and are essential for " +
                       "the success of my spell. In return for your efforts, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a unique Arcane Gloves that will enhance your mystical prowess.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, seek me out again. The arcane awaits!"; } }

        public override object Uncomplete { get { return "I still require 50 Arcanic Rune Stones. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Marvelous! You've gathered all the Arcanic Rune Stones I need. My ritual will be a great success thanks to your aid. " +
                       "Accept these rewards as a token of my appreciation. May your magical journeys be ever prosperous!"; } }

        public ArcanicRuneCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ArcanicRuneStone), "Arcanic Rune Stone", 50, 0x573C)); // Assuming Arcanic Rune Stone item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SousChefsPrecisionGloves), 1, "Arcane Gloves")); // Assuming Arcane Mantle is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Arcanic Rune Hunt quest!");
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

    public class ArcanicRuneCollectorEldara : MondainQuester
    {
        [Constructable]
        public ArcanicRuneCollectorEldara()
            : base("The Enigmatic Sorceress", "Eldara")
        {
        }

        public ArcanicRuneCollectorEldara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = 0x497; // Magical blue hue
            HairItemID = 0x203B; // Mystical hair style
            HairHue = 0x48E; // Hair hue (deep purple)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x1F5)); // Mystical robe
            AddItem(new Sandals(0x1F5)); // Matching sandals
            AddItem(new HoodedShroudOfShadows { Name = "Eldara's Enigmatic Hood", Hue = 0x1F5 }); // Custom hood
            AddItem(new Cloak { Name = "Eldara's Arcane Mantle", Hue = 0x1F5 }); // Custom arcane mantle
            AddItem(new BlackStaff { Name = "Eldara's Enchanted Staff", Hue = 0x1F5 }); // Custom staff
            Backpack backpack = new Backpack();
            backpack.Hue = 0x1F5;
            backpack.Name = "Bag of Arcane Relics";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ArcanicRuneCollectorQuest)
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
