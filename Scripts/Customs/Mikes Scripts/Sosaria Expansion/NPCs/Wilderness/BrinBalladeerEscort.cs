using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class BrinBalladeerQuest : BaseQuest
	{
		public override object Title { get { return "Song of the Fading Voice"; } }

		public override object Description
		{
			get
			{
				return 
					"*The bard before you coughs, his voice hoarse yet still carrying a haunting melody.*\n\n" +
					"'Traveler... you see, I am Brin, called the Balladeer. My voice—my gift—is fading, stolen by a curse I cannot undo alone. I must reach Fawn Inn before nightfall, for there, my final song may yet find its end... and perhaps, my freedom.'\n\n" +
					"*He looks into the distance, eyes filled with desperation and resolve.*\n\n" +
					"'Escort me safely, and I shall see that you are rewarded with the blessings of melody and swift steps... Please, my time wanes.'";
			}
		}

		public override object Refuse { get { return "'Without your aid... the silence will take me. I pray another soul will listen.'"; } }
		public override object Uncomplete { get { return "'Please, friend, Fawn Inn lies not far... and yet it feels like leagues from here.'"; } }

		public BrinBalladeerQuest() : base()
		{
			AddObjective(new EscortObjective("Fawn Inn"));
			AddReward(new BaseReward(typeof(HarmonistsSoftShoes), "Harmonist's Soft Shoes"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("Brin breathes deeply, his voice returning in a soft melody. 'You have saved not just a man, but a song. Take these shoes, may they carry you swiftly, as your kindness carried me.'", null, 0x59B);
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

	public class BrinBalladeerEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(BrinBalladeerQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBBard());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public BrinBalladeerEscort() : base()
		{
			Name = "Brin";
			Title = "the Balladeer";
			NameHue = 0x83F;
		}

		public BrinBalladeerEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(45, 60, 30);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83F8;
			HairItemID = 0x2049;
			HairHue = 1109;
		}

		public override void InitOutfit()
		{
			AddItem(new FancyShirt() { Hue = 0x482 });
			AddItem(new ElvenPants() { Hue = 0x47E });
			AddItem(new Cloak() { Hue = 0x59B });
			AddItem(new FeatheredHat() { Hue = 0x59B });
			AddItem(new Sandals() { Hue = 0x487 });
			AddItem(new ResonantHarp());
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
						"*Brin hums softly, but the tune falters into silence.*",
						"'The road is long... but a song shortens any journey, or so I once believed.'",
						"*He clutches his throat* 'It fades... the curse tightens. Please, we must hasten.'",
						"'Fawn's hearth waits for me, as do the notes I’ve yet to sing.'",
						"*Brin stares into the sky* 'Even the stars seem to listen no more.'",
						"'Have you ever feared forgetting your own voice?'",
						"*He coughs, then offers a brief, haunting verse... before silence returns.*"
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

	public class HarmonistsSoftShoes : Sandals
	{
		[Constructable]
		public HarmonistsSoftShoes() : base()
		{
			Hue = 0x47E;
			Name = "Harmonist's Soft Shoes";
			Attributes.BonusDex = 2;
			SkillBonuses.SetValues(0, SkillName.Musicianship, 5.0);
		}

		public HarmonistsSoftShoes(Serial serial) : base(serial) { }

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
