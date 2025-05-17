using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScalesOfHeresyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Scales of Heresy"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Liora Gemgleam*, the renowned Jeweler of Dawn.\n\n" +
                    "Her eyes glisten like the finest gemstones, but shadows linger beneath them.\n\n" +
                    "“Have you seen a drake marked with infernal sigils? It took my life’s work—precious gems meant for the Blossomcircle festival. The beast lairs in Doom, guarding what it stole. Its scales shimmer with heresy, and its breath reeks of brimstone.”\n\n" +
                    "“Bring me its head, and recover what’s mine. These stones aren’t just jewels—they hold the spirit of Dawn.”\n\n" +
                    "**Slay the Cult Drake** that guards the vault in Doom, and recover the gems of Dawn.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand. But without those gems, Dawn may lose more than beauty—we may lose our spirit.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Cult Drake still breathes? Then our gems remain lost in fire and heresy. Please, act before it vanishes again!";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain the beast? And the gems… they are here, safe at last. You’ve done more than I hoped. Please, take this *BlossomcircleOfDawn*—a token of our rebirth.";
            }
        }

        public ScalesOfHeresyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultDrake), "Cult Drake", 1));
            AddReward(new BaseReward(typeof(BlossomcircleOfDawn), 1, "BlossomcircleOfDawn"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Scales of Heresy'!");
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
	
	public class LioraGemgleam : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(ScalesOfHeresyQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBJewel());
		}

		[Constructable]
		public LioraGemgleam()
			: base("the Gemwright", "Liora Gemgleam")
		{
		}

		public LioraGemgleam(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 70, 25);

			Female = true;
			Body = 0x191; // Female
			Race = Race.Human;

			Hue = Race.RandomSkinHue();
			HairItemID = 0x203B; // Long Hair
			HairHue = 1153; // Deep Sapphire Blue
		}

		public override void InitOutfit()
		{
			AddItem(new FancyDress() { Hue = 1157, Name = "Twilight-Silk Gown" }); // Ethereal purple
			AddItem(new Cloak() { Hue = 1372, Name = "Moonlight Cloak" }); // Soft silver
			AddItem(new Sandals() { Hue = 2419, Name = "Gleamstep Sandals" }); // Polished Jade
			AddItem(new FlowerGarland() { Hue = 1150, Name = "Gemblossom Circlet" }); // Crystal white
			AddItem(new BodySash() { Hue = 1289, Name = "Ember-Sewn Sash" }); // Light coral

			Backpack backpack = new Backpack();
			backpack.Hue = 1161; // Deep Ruby
			backpack.Name = "Gemwright's Pack";
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
