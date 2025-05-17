using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class BruskIronbrowQuest : BaseQuest
	{
		public override object Title { get { return "Echoes in the Stone"; } }

		public override object Description
		{
			get
			{
				return 
					"You there! You’ve got eyes, and more importantly, ears that ain’t filled with voices, right? I’m Brusk Ironbrow, miner from Devil Guard... or what’s left of me. I’ve found something, something big, but they won’t let me go easy. The voices, I mean. They whisper, follow... Please, get me back to Devil Guard before I lose more than my mind!";
			}
		}

		public override object Refuse { get { return "Fine, leave me here with the stones. Maybe they’ll talk sense into me... or take me for good."; } }
		public override object Uncomplete { get { return "Hurry! They’re closer now—I can feel them breathing down my neck!"; } }

		public BruskIronbrowQuest() : base()
		{
			AddObjective(new EscortObjective("Devil Guard"));
			AddReward(new BaseReward(typeof(VolendrungWorHammer), "Volendrung WorHammer"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("You’ve done me a great turn, friend. The voices... they’re fading. Take this, may your hands be swift, but mind your strength. The stone always takes its due.", null, 0x59B);
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

	public class BruskIronbrowEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(BruskIronbrowQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBMiner());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public BruskIronbrowEscort() : base()
		{
			Name = "Brusk Ironbrow";
			Title = "the Paranoid Prospector";
			NameHue = 0x83F;
		}

		public BruskIronbrowEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 50, 40);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83EA;
			HairItemID = 0x2049;
			HairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new HalfApron() { Hue = 0x976 });
			AddItem(new LeatherGloves());
			AddItem(new LeatherArms());
			AddItem(new LeatherLegs());
			AddItem(new Boots());
			AddItem(new LeatherCap());
			AddItem(new Pickaxe());
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
						"*Brusk tightens his grip on his pickaxe* 'They’re close... can’t you feel the ground hum?'",
						"*He mutters under his breath* 'You can’t have it! The ore’s mine!'",
						"'Keep moving, don’t stop... they don’t like when we stop.'",
						"*Brusk flinches suddenly* 'That voice again... whisperin’ about the dark.'",
						"'You see any shadows? I swear they’re following us!'",
						"*He pats a pouch at his side* 'Still there... good, still there.'"
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