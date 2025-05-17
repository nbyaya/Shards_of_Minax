using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class MalrickQuest : BaseQuest
	{
		public override object Title { get { return "Rest for the Restless"; } }

		public override object Description
		{
			get
			{
				return 
					"The dead stir uneasily, friend. I must return to Death Gulch, but I dare not walk alone—the spirits know me, and not all are kind. Escort me home before the blood moon, and I shall reward you with a rune of virtue, imbued with the blessings of those long passed.";
			}
		}

		public override object Refuse { get { return "The dead are patient... but not forever."; } }
		public override object Uncomplete { get { return "We are not safe yet. Their whispers grow louder."; } }

		public MalrickQuest() : base()
		{
			AddObjective(new EscortObjective("Death Gulch"));
			AddReward(new BaseReward(typeof(VirtueRune), "VirtueRune – Rune of the Restful"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("Thank you. The dead rest easier now... and so shall you, knowing virtue still lives. Take this, a small token from the silent.", null, 0x59B);
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

	public class MalrickEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(MalrickQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBNecromancer());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public MalrickEscort() : base()
		{
			Name = "Malrick";
			Title = "the Gravekeeper";
			NameHue = 0x83F;
		}

		public MalrickEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 50, 40);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83EA;
			HairItemID = 0x2049;
			HairHue = 1150;
			FacialHairItemID = 0x203C;
			FacialHairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new MonkRobe() { Hue = 0x455 });
			AddItem(new Cloak() { Hue = 0x497 });
			AddItem(new LeatherGloves() { Hue = 0x497 });
			AddItem(new Boots() { Hue = 0x4B5 });
			AddItem(new WideBrimHat() { Hue = 0x1 });
			AddItem(new ShepherdsCrook());
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
						"*Malrick clutches his lantern tightly* 'Did you see that? A shadow... no, just the wind... perhaps.'",
						"'The path is familiar, yet the air feels... wrong. Something follows us.'",
						"*He mutters an incantation* 'Sleep now, old bones... not yet, not yet.'",
						"'I was their voice in life, their keeper in death. I will not abandon them again.'",
						"*Malrick glances skyward* 'The moon waxes... we have little time.'"
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