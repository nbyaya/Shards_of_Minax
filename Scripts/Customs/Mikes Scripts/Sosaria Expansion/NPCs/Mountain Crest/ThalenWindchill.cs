using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ThreadsOfWinterQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Threads of Winter"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Thalen Windchill*, Mystic of the Mountain.\n\n" +
                    "Clad in robes shimmering with frost and laced with ancient runes, his breath steams even in the cold air.\n\n" +
                    "“Ice is a living force. We shape it, but it can shape us in return.”\n\n" +
                    "“A sorcerer once stood where you stand, weaving frost as a guardian of balance. But he has fallen—become the *Frostwoven Mage*, binding frozen motes into wicked spells.”\n\n" +
                    "“He defiles the caverns I once called sacred. His runes are traps; his breath, a storm.”\n\n" +
                    "**Venture into the Ice Cavern** and **slay the Frostwoven Mage** before his magic corrupts the mountain’s heart.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the cold, traveler. It does not forgive hesitation, and neither will he.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You still feel the chill, don’t you? He lives, and his magic deepens its hold.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The frost weakens… you have undone a terrible weaving.\n\n" +
                       "Take this: the *WindripperBow*. Crafted from mountain ash, it sings with the breath of winter and will serve you well.";
            }
        }

        public ThreadsOfWinterQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostwovenMage), "Frostwoven Mage", 1));
            AddReward(new BaseReward(typeof(WindripperBow), 1, "WindripperBow"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Threads of Winter'!");
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

    public class ThalenWindchill : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ThreadsOfWinterQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        [Constructable]
        public ThalenWindchill()
            : base("the Mystic of the Mountain", "Thalen Windchill")
        {
        }

        public ThalenWindchill(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 80, 100);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Long hair
            HairHue = 1152; // Icy white-blue
            FacialHairItemID = 0x204B; // Long beard
            FacialHairHue = 1152;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Frostbound Cloak" });
            AddItem(new Robe() { Hue = 1154, Name = "Runed Winter Robe" });
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Chilltouch Gloves" });
            AddItem(new Sandals() { Hue = 1153, Name = "Snowdrift Sandals" });
            AddItem(new WizardsHat() { Hue = 1152, Name = "Helm of Icewisdom" });

            AddItem(new MagicWand() { Hue = 1150, Name = "Shardweaver Wand" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Mystic’s Satchel";
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
