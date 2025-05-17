using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LightInTheDarknessQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Light in the Darkness"; } }

        public override object Description
        {
            get
            {
                return
                    "Nella Waxbloom, the candle-maker of Dawn, greets you with a worried smile.\n\n" +
                    "“In the chapel, where the light of my candles should bring peace, I’ve seen a shadow moving through the stained glass. A knight, or something like one, cursed and cloaked in waxen gloom. They say it came from the Doom Dungeon, and now it haunts the chapel’s heart.”\n\n" +
                    "“Please, brave one... I craft light to push back the dark, but this is beyond my hands. **Slay the Cursed Solen Warrior** that pollutes this sacred place.”\n\n" +
                    "“Return with peace, and I shall reward you with something woven from stillness and flame.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Even candles cannot chase this shadow alone… I’ll keep praying that someone will be strong enough to bring light.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“The shadow still lingers? I feel its chill every night… My candles flicker, afraid.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“You’ve done it! The chapel breathes again… My candles burn true, and the shadows no longer creep. Please, take this *StillwaterUndergarment*. It holds the peace you restored.”";
            }
        }

        public LightInTheDarknessQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedSolenWarrior), "Cursed Solen Warrior", 1));
            AddReward(new BaseReward(typeof(StillwaterUndergarment), 1, "StillwaterUndergarment"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Light in the Darkness'!");
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

    public class NellaWaxbloom : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(LightInTheDarknessQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBeekeeper());
        }

        [Constructable]
        public NellaWaxbloom()
            : base("the Candle-Maker", "Nella Waxbloom")
        {
        }

        public NellaWaxbloom(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 20);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long Hair
            HairHue = 1153; // Soft lavender-gray
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 1157, Name = "Wax-Streaked Dress" }); // Pale honey
            AddItem(new Cloak() { Hue = 1175, Name = "Glimmering Waxen Cloak" }); // Soft golden shimmer
            AddItem(new Sandals() { Hue = 1109, Name = "Smoke-Touched Sandals" }); // Charcoal gray
            AddItem(new FlowerGarland() { Hue = 1154, Name = "Bloomed Wax Garland" }); // Candlewax-white
            AddItem(new BodySash() { Hue = 1160, Name = "Ribbon of Flickering Light" }); // Pale gold

            Backpack backpack = new Backpack();
            backpack.Hue = 1170;
            backpack.Name = "Candle Maker’s Pack";
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
