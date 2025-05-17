using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class WhisperwindLocketQuest : BaseQuest
	{
		public override object Title { get { return "Whispers of the Past"; } }

		public override object Description
		{
			get
			{
				return 
					"I feel it... pulling me back. I am Nella Whisperwind, and I must return to the home I fled long ago. Within its walls lies a locket—my family's secret, my only hope for peace. But the shadows there are restless. Will you escort me? I fear what waits within, but I cannot leave it behind.";
			}
		}

		public override object Refuse { get { return "Then I shall wait, as I always have, caught between what was and what might be."; } }
		public override object Uncomplete { get { return "The shadows grow bolder. Please, we must hurry, before the past swallows me whole."; } }

		public WhisperwindLocketQuest() : base()
		{
			AddObjective(new EscortObjective("an Abandoned Home"));
			AddReward(new BaseReward(typeof(JestersMerryCap), "Jester's Merry Cap"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("Thank you... With the locket, I can remember, and perhaps, forget. Take this cap, may it lighten your heart as you've lightened mine.", null, 0x59B);
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

	public class NellaWhisperwindEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(WhisperwindLocketQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBMystic());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public NellaWhisperwindEscort() : base()
		{
			Name = "Nella Whisperwind";
			Title = "the Haunted Mystic";
			NameHue = 0x83E;
		}

		public NellaWhisperwindEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 60, 30);
			Female = true;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83EA;
			HairItemID = 0x203C; // Long Hair
			HairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new FancyDress() { Hue = 0x497 });
			AddItem(new Cloak() { Hue = 0x4F1 });
			AddItem(new Sandals() { Hue = 0x453 });
			AddItem(new SkullCap() { Hue = 0x482 });
			AddItem(new MagicWand());
		}

		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now >= m_NextTalkTime && this.Controlled)
			{
				if (Utility.RandomDouble() < 0.1) // 10% chance to talk each tick
				{
					string[] lines = new string[]
					{
						"*Nella's eyes flicker with unease* 'Do you hear them? The whispers... they never truly stopped.'",
						"'This path feels... familiar. Like a dream half-remembered, or a nightmare I wish to forget.'",
						"'My locket holds more than memories. It holds a truth I dare not speak.'",
						"*She clutches her chest* 'It calls to me, the house. But will it let me leave again?'",
						"'Thank you for walking beside me. Few would dare the shadows of another's past.'",
						"*Her gaze lingers on the horizon* 'Do not trust the silence. That is when they come.'",
						"'Soon, we will see the house. Soon, I will know if I am strong enough to face it.'"
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
