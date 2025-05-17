using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class TorbinOakhandQuest : BaseQuest
	{
		public override object Title { get { return "The Whispering Woods"; } }

		public override object Description
		{
			get
			{
				return
					"*Torbin wipes his brow and glances nervously over his shoulder.*\n" +
					"Name's Torbin Oakhand, lumberjack by trade... lately haunted, it seems. I was deep in the groves, chopping oak as usual, when something... followed me home. A voice in the trees, cold hands on my neck at night. I need to reach Yew Inn to meet a priest who can cleanse me of this forest curse. Please, walk with me—before the spirit decides I belong to the woods.";
			}
		}

		public override object Refuse { get { return "I can’t linger long... the trees are already calling. I fear what happens if I wait."; } }
		public override object Uncomplete { get { return "Please, I must reach Yew Inn soon... it’s getting darker, even in daylight."; } }

		public TorbinOakhandQuest() : base()
		{
			AddObjective(new EscortObjective("Yew Inn"));
			AddReward(new BaseReward(typeof(WatermelonSliced), "a refreshing slice of watermelon"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("You’ve saved me from the forest’s grasp. Thank you, friend. May this keep you cool and strong on your travels.", null, 0x59B);
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

	public class TorbinOakhandEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(TorbinOakhandQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBAxeWeapon());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public TorbinOakhandEscort() : base()
		{
			Name = "Torbin Oakhand";
			Title = "the Haunted Lumberjack";
			NameHue = 0x83F;
		}

		public TorbinOakhandEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 50, 45);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83EA;
			HairItemID = 0x2049;
			HairHue = 1109;
			FacialHairItemID = 0x203F;
			FacialHairHue = 1109;
		}

		public override void InitOutfit()
		{
			AddItem(new FancyShirt() { Hue = 0x59B });
			AddItem(new LeatherShorts() { Hue = 0x842 });
			AddItem(new HalfApron() { Hue = 0x47E });
			AddItem(new Boots() { Hue = 0x497 });
			AddItem(new BearMask() { Hue = 0x455 });
			AddItem(new Axe());
		}

		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now >= m_NextTalkTime && this.Controlled)
			{
				if (Utility.RandomDouble() < 0.1)
				{
					string[] lines = new string[]
					{
						"*Torbin shudders* 'I swear the trees are whispering... listen close.'",
						"'The deeper I chopped, the colder it got. Like the forest was watching me.'",
						"*Torbin grips his axe tightly* 'This old tool’s saved me before, but not from spirits.'",
						"'We’re close, right? I can’t keep this curse at bay much longer.'",
						"*He glances at the trees* 'Did you see that? Something moved—something *wrong*.'",
						"*Torbin rubs his hands together* 'They say the forest takes what it’s owed... I hope I’m not part of the debt.'",
						"'Please, don’t leave me behind. I don’t want to go back there.'"
					};

					Say(lines[Utility.Random(lines.Length)]);
					m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_NextTalkTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_NextTalkTime = reader.ReadDateTime();
		}
	}
}
