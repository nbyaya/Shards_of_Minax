using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FireRubyCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Fiery Gem Collector"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave adventurer! I am Ignis, the Flamecaller. My flames are waning, and I need your aid to rekindle them. " +
                       "I require 50 FireRubies, gems imbued with the essence of elemental fire. These precious stones are vital to restore my " +
                       "power and protect the realm from encroaching darkness. In exchange for your valor, I shall grant you gold, a rare Maxxia Scroll, " +
                       "and a magnificent Flamecaller’s Mantle that will surely make you the envy of all who behold it.";
            }
        }

        public override object Refuse { get { return "As you wish. Should you change your mind, return with the FireRubies."; } }

        public override object Uncomplete { get { return "I still require 50 FireRubies. Bring them to me, and we shall vanquish the darkness together!"; } }

        public override object Complete { get { return "Splendid work! You have collected the 50 FireRubies I needed. Your bravery is unmatched. Accept these rewards with my deepest gratitude, " +
                       "and may your path be forever illuminated by the flames of valor!"; } }

        public FireRubyCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FireRuby), "FireRubies", 50, 0x3197)); // Assuming FireRuby item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DuelistsLegplates), 1, "Flamecaller’s Mantle")); // Assuming Flamecaller’s Mantle is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Fiery Gem Collector quest!");
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

    public class FlamecallerIgnis : MondainQuester
    {
        [Constructable]
        public FlamecallerIgnis()
            : base("The Flamecaller", "Ignis")
        {
        }

        public FlamecallerIgnis(Serial serial)
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
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Ignis’s Flame Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Ignis’s Flamecaller’s Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Ignis’s Fiery Ring" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Ignis’s Flame Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Ignis’s Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FireRubyCollectorQuest)
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
