using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class LardOfParoxysmusQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Lard of Paroxysmus"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Marnel the Sage, an ancient keeper of lost knowledge. I seek your aid in a crucial task. " +
                       "Long ago, the mystical Lard of Paroxysmus was scattered across the realms. These enchanted morsels are essential for " +
                       "my arcane rituals to unlock forgotten secrets of the cosmos. I need you to gather 50 pieces of Lard of Paroxysmus for me. " +
                       "In gratitude, I shall reward you with gold, a rare Maxxia Scroll, and an enchanted outfit that will mark you as a true " +
                       "friend of the arcane arts.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Lard of Paroxysmus."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of Lard of Paroxysmus. Please bring them to me so I can proceed with my rituals!"; } }

        public override object Complete { get { return "Excellent! You have brought me the 50 pieces of Lard of Paroxysmus I needed. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my appreciation. May your path be filled with arcane wonders!"; } }

        public LardOfParoxysmusQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(LardOfParoxysmus), "Lard of Paroxysmus", 50, 0x3189)); // Assuming LardOfParoxysmus item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ShogunsAuthoritativeSurcoat), 1, "Marnel's Enchanted Surcoat")); // Assuming Marnel's Enchanted Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Lard of Paroxysmus quest!");
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

    public class MarnelTheSage : MondainQuester
    {
        [Constructable]
        public MarnelTheSage()
            : base("The Arcane Sage", "Marnel")
        {
        }

        public MarnelTheSage(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest { Hue = Utility.Random(1, 3000), Name = "Marnel's Arcane Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Marnel's Mystical Hat" });
            AddItem(new Crossbow { Hue = Utility.Random(1, 3000), Name = "Marnel's Enchanted Wand" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Marnel's Arcane Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Marnel's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LardOfParoxysmusQuest)
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
