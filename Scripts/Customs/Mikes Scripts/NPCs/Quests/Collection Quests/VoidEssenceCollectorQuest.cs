using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class VoidEssenceCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Void Essence Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Xaroth, the Master of Shadows. A troubling disturbance has plagued " +
                       "my domain, and I require your assistance. I need you to collect 50 VoidEssence, which will help me " +
                       "stabilize the rifts and restore balance to the shadow realm. In return for your valiant efforts, you shall " +
                       "be rewarded with gold, a rare Maxxia Scroll, and the esteemed Voidmaster's Cloak, which will mark you as " +
                       "a hero of the shadows.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, come back with the VoidEssence."; } }

        public override object Uncomplete { get { return "You still need to gather 50 VoidEssence. Return to me once you have collected them all!"; } }

        public override object Complete { get { return "Well done! You have collected the 50 VoidEssence I required. Your courage is commendable. Please accept these " +
                       "rewards as a token of my gratitude. May the shadows guide your path!"; } }

        public VoidEssenceCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(VoidEssence), "VoidEssence", 50, 0x4007)); // Assuming VoidEssence item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RoguesShadowCloak), 1, "Voidmaster's Cloak")); // Assuming Voidmaster's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Void Essence Connoisseur quest!");
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

    public class VoidmasterXaroth : MondainQuester
    {
        [Constructable]
        public VoidmasterXaroth()
            : base("The Voidmaster", "Xaroth")
        {
        }

        public VoidmasterXaroth(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Xaroth's Shadow Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Xaroth's Enigmatic Hat" });
            AddItem(new SilverBracelet { Hue = Utility.Random(1, 3000), Name = "Xaroth's Arcane Bracelet" });
            AddItem(new StuddedGloves { Hue = Utility.Random(1, 3000), Name = "Xaroth's Shadow Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Xaroth's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(VoidEssenceCollectorQuest)
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
