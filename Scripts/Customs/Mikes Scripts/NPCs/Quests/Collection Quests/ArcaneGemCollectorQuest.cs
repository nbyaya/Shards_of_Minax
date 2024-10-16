using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ArcaneGemCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Arcane Gem Hunt"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Alaric, a master of arcane arts and collector of rare mystical artifacts. " +
                       "My collection is missing one crucial element—Arcane Gems. I need 50 of these gems to complete my mystical array. " +
                       "If you can bring them to me, I will reward you with gold, a rare Maxxia Scroll, and a one-of-a-kind Arcane Tekko that " +
                       "radiates with magical energy. Will you assist me in this quest?";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, return to me with the Arcane Gems. The magic awaits!"; } }

        public override object Uncomplete { get { return "I still need 50 Arcane Gems to complete my collection. Please bring them to me soon!"; } }

        public override object Complete { get { return "You have done it! The Arcane Gems are now part of my collection. As a token of my gratitude, " +
                       "please accept these rewards. Your name will be remembered in the annals of arcane history."; } }

        public ArcaneGemCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ArcaneGem), "Arcane Gem", 50, 0x1EA7)); // Assuming ArcaneGem item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DaimyosHonorTekko), 1, "Arcane Tekko")); // Assuming Arcane Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Arcane Gem Hunt quest!");
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

    public class ArcaneGemCollectorAlaric : MondainQuester
    {
        [Constructable]
        public ArcaneGemCollectorAlaric()
            : base("The Arcane Sage", "Arcane Gem Collector Alaric")
        {
        }

        public ArcaneGemCollectorAlaric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Mage's hair style
            HairHue = 1157; // Hair hue (silver)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1157) { Name = "Alaric's Mystical Robe" }); // Custom Robe
            AddItem(new Sandals(1157)); // Matching sandals
            AddItem(new WizardsHat { Name = "Alaric's Arcane Hat", Hue = 1157 }); // Custom Arcane Hat
            AddItem(new GoldRing { Name = "Arcane Amulet", Hue = 1157 }); // Custom Amulet
            AddItem(new GnarledStaff { Name = "Alaric's Enchanted Staff", Hue = 1157 }); // Custom Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Bag of Mystical Artifacts";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ArcaneGemCollectorQuest)
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
