using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MischiefManagedQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mischief Managed"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Bryony Willowshard*, the renowned jeweler of Fawn, amidst a dazzling array of gems and curious trinkets.\n\n" +
                    "Her workshop glimmers, but there’s tension in her hands as she adjusts a display of enchanted stones.\n\n" +
                    "“It’s that *Nimogwai* again. The little beast adores my gems, can’t resist them. It's been slipping past my traps, stealing shards of my finest work!”\n\n" +
                    "“I’ve scattered lures—glittering ore, enchanted baubles—but this time, I want it gone for good.”\n\n" +
                    "“Find the Nimogwai, end its mischief, and bring peace back to my workshop. Do this, and you’ll have my gratitude—and a chest no thief could ever claim.”\n\n" +
                    "**Hunt down the Nimogwai in the Wilderness before it steals more of Bryony’s treasures.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your pockets stay light, and my gems stay hidden from prying claws.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no sign of the Nimogwai? It's getting bolder—I found a sapphire half-bitten this morning!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Gone? Truly? Then I can finally breathe easy.\n\n" +
                       "You’ve done me a great favor. Take this—**the RebelChest**. It’s said to refuse all but its rightful owner. May it keep your treasures safer than mine ever were.";
            }
        }

        public MischiefManagedQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Nimogwai), "Nimogwai", 1));
            AddReward(new BaseReward(typeof(RebelChest), 1, "RebelChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mischief Managed'!");
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

    public class BryonyWillowshard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MischiefManagedQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        [Constructable]
        public BryonyWillowshard()
            : base("the Gemwright", "Bryony Willowshard")
        {
        }

        public BryonyWillowshard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Wavy long hair
            HairHue = 1358; // Lustrous violet
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Sapphire-Touched Blouse" });
            AddItem(new Skirt() { Hue = 1175, Name = "Twilight Silk Skirt" });
            AddItem(new BodySash() { Hue = 1154, Name = "Gleaming Gem Sash" });
            AddItem(new Sandals() { Hue = 1172, Name = "Soft-Step Sandals" });
            AddItem(new FeatheredHat() { Hue = 1178, Name = "Willowshade Hat" });

            AddItem(new ArtificerWand() { Hue = 1270, Name = "Gem-Tuner’s Wand" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Jeweler’s Kit";
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
