using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CrushedGlassCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Shattered Artistry"; } }

        public override object Description
        {
            get
            {
                return "Greetings, traveler! I am Alaric, a master glassblower of great renown. My latest creation requires " +
                       "a unique ingredient: CrushedGlass. I need 50 pieces of this delicate substance to complete my work. " +
                       "Can you assist me in gathering these shards? In return, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and a one-of-a-kind Glassblower's Outfit that will surely set you apart!";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me. My masterpiece awaits!"; } }

        public override object Uncomplete { get { return "I still require 50 pieces of CrushedGlass to complete my creation. Please bring them to me."; } }

        public override object Complete { get { return "You've gathered all the CrushedGlass I need! My work will be completed thanks to your effort. " +
                       "Please accept these rewards as a token of my gratitude. Thank you!"; } }

        public CrushedGlassCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CrushedGlass), "CrushedGlass", 50, 0x573B)); // Assuming CrushedGlass item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HelmOfTheAbyss), 1, "Glassblower's Hat")); // Custom item
            AddReward(new BaseReward(typeof(AmbassadorsCloak), 1, "Glassblower's Cloak")); // Custom item
            AddReward(new BaseReward(typeof(ArchivistsShoes), 1, "Glassblower's Shoes")); // Custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Shattered Artistry quest!");
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

    public class CrushedGlassCollectorAlaric : MondainQuester
    {
        [Constructable]
        public CrushedGlassCollectorAlaric()
            : base("The Glassblower", "Alaric the Glassblower")
        {
        }

        public CrushedGlassCollectorAlaric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Hair style
            HairHue = 1151; // Hair hue (light blue)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1150)); // Base robe
            AddItem(new Sandals(1150)); // Base sandals
            AddItem(new FeatheredHat(1150)); // Glassblower's hat
            AddItem(new Robe { Name = "Alaric's Glassblower Robe", Hue = 1150 }); // Custom robe
            AddItem(new Shoes { Name = "Alaric's Glassblower Shoes", Hue = 1150 }); // Custom shoes
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Glassblowing Tools";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CrushedGlassCollectorQuest)
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
