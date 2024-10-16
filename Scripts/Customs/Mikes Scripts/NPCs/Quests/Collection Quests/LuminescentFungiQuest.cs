using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class LuminescentFungiQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Glowing Fungi"; } }

        public override object Description
        {
            get
            {
                return "Ah, a brave soul! I am Nyxara, the Guardian of the Mystical Glade. " +
                       "The forest has been suffering lately, and I have discovered that the Luminescent Fungi are disappearing. " +
                       "These fungi are not only beautiful but essential to the balance of our mystical forest. I need your help to " +
                       "collect 50 Luminescent Fungi so that I can restore the natural harmony. In return, I shall grant you gold, " +
                       "a rare Maxxia Scroll, and a special enchanted Gloves, woven with the essence of the forest itself.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, bring the Luminescent Fungi to me."; } }

        public override object Uncomplete { get { return "I still need 50 Luminescent Fungi to restore the balance of the forest. Please bring them to me!"; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Luminescent Fungi I required. The forest will soon be at peace again. " +
                       "Please accept these rewards as a token of my gratitude. May the forest's magic guide your path!"; } }

        public LuminescentFungiQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(LuminescentFungi), "Luminescent Fungi", 50, 0x3191)); // Assuming Luminescent Fungi item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GlovesOfStonemasonry), 1, "Enchanted Forest Gloves")); // Assuming Enchanted Forest Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed 'The Glowing Fungi' quest!");
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

    public class GuardianNyxara : MondainQuester
    {
        [Constructable]
        public GuardianNyxara()
            : base("The Guardian of the Mystical Glade", "Nyxara")
        {
        }

        public GuardianNyxara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Nyxara's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Nyxara's Enchanted Crown" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Nyxara's Forest Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(LuminescentFungiQuest)
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
