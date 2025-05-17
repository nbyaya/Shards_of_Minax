using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class SisterDellaQuest : BaseQuest
	{
		public override object Title { get { return "In Dust and Shadow"; } }

		public override object Description
		{
			get
			{
				return "Sister Della needs safe escort to Death Gulch Inn, where a relic broker awaits her with sacred (or cursed) artifacts. Her knowledge of forbidden texts has earned her enemies—protect her from both monsters and man.";
			}
		}

		public override object Refuse { get { return "Even saints must walk alone, sometimes."; } }
		public override object Uncomplete { get { return "The sands grow restless, my friend. We must move on."; } }

		public SisterDellaQuest() : base()
		{
			AddObjective(new EscortObjective("Death Gulch Inn"));
			AddReward(new BaseReward(typeof(EnchantedAnnealer), "EnchantedAnnealer"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("Thank you, child. May the relic serve more than it deceives. Take this, a token of tempered wisdom.", null, 0x59B);
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

	public class SisterDellaEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(SisterDellaQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBHolyMage());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public SisterDellaEscort() : base()
		{
			Name = "Sister Della";
			Title = "of the Dust";
			NameHue = 0x83F;
		}

		public SisterDellaEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(70, 60, 30);
			Female = true;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x8401;
			HairItemID = 0x2048; // Long hair
			HairHue = 1150; // Dusty Grey
		}

		public override void InitOutfit()
		{
			AddItem(new MonkRobe() { Hue = 0x47E }); // Dusty robe
			AddItem(new Sandals() { Hue = 0x59B }); // Pale leather sandals
			AddItem(new HoodedShroudOfShadows() { Hue = 0x47E }); // Hooded look, dusty color
			AddItem(new HolyKnightSword()); // She carries a sacred relic blade, more ceremonial than combative.
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
						"*Della brushes dust from her robe* 'Each grain, a memory forgotten... or hidden.'",
						"'Relics do not choose their bearers lightly. Nor do I.'",
						"'This road... I have walked it before, in dreams turned nightmares.'",
						"'The dust knows my name. It clings, even to the soul.'",
						"*She clutches a pendant tightly* 'We are almost there, and still I feel the pull...'",
						"'Do you believe in fate? Or do we craft it, with every step?'"
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