using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MinotaursHornQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Minotaur’s Horn"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Lyra Shoals*, Renika's renowned artisan jeweler, known for weaving the sea’s bounty into radiant treasures.\n\n" +
                    "Her hands, adorned with rings of pearl and coral, clasp a half-finished necklace—a space left for something... grander.\n\n" +
                    "“I’ve shaped the shimmer of the sea, the brilliance of gems, but none compare to the legend of the **MountainMinotaur’s spiral horn**. " +
                    "It’s said to hold a light within, brighter than any jewel. Rumors have grown louder, eclipsing my craft. Buyers want stories now, not just beauty.”\n\n" +
                    "“Bring me that horn. Prove the legends true—or false. Let me shape it into something eternal, a masterpiece none can deny.”\n\n" +
                    "**Slay the MountainMinotaur** in the Mountain Stronghold and return with its spiral horn.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "So be it. The seas will give, as they always have, but perhaps they can’t rival the mountain’s call.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still hunting? That horn casts a long shadow. I can feel its weight from here, even unfinished.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The horn... it's magnificent. The rumors didn’t lie.\n\n" +
                       "Now, let the world see my craft shine again.\n\n" +
                       "Take this: an **ExoticFish**, rare and vibrant, caught only in the deepest of tides. A token from the sea, for braving the mountain.";
            }
        }

        public MinotaursHornQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MountainMinotaur), "MountainMinotaur", 1));
            AddReward(new BaseReward(typeof(ExoticFish), 1, "ExoticFish"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Minotaur’s Horn'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class LyraShoals : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MinotaursHornQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel());
        }

        [Constructable]
        public LyraShoals()
            : base("the Artisan Jeweler", "Lyra Shoals")
        {
        }

        public LyraShoals(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Light sun-touched skin
            HairItemID = 0x2047; // Long Hair
            HairHue = 1147; // Seafoam green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Pearl-Spun Blouse" }); // Soft teal
            AddItem(new LongPants() { Hue = 2401, Name = "Deepsea Weave Trousers" }); // Dark indigo
            AddItem(new HalfApron() { Hue = 2101, Name = "Coral-Threaded Apron" }); // Coral pink
            AddItem(new Sandals() { Hue = 1109, Name = "Tidewalker Sandals" }); // Grey-blue
            AddItem(new FeatheredHat() { Hue = 1155, Name = "Shellbound Hat" }); // Light sky-blue
            AddItem(new BodySash() { Hue = 2105, Name = "Ocean's Embrace Sash" }); // Pale rose

            AddItem(new GoldRing() { Name = "Pearl Ring", Hue = 2107 });
            AddItem(new Beads() { Name = "Sea Glass Necklace", Hue = 1152 });

            Backpack backpack = new Backpack();
            backpack.Hue = 1171; // Soft aqua
            backpack.Name = "Jeweler's Pack";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
