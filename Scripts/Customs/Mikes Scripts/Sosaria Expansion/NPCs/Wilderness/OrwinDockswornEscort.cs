using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class OrwinDockswornQuest : BaseQuest
	{
		public override object Title { get { return "Verses of the Sea"; } }

		public override object Description
		{
			get
			{
				return 
					"*Orwin clutches a worn parchment* 'The sea... she won't let me rest. Her songs fill my head, tides of words I can't hold back. A bard waits for me at Trinsic Inn. He claims he can set the verses free, give voice to what I fear. Will you walk with me? Keep the sea's ghosts at bay while we go?'";
			}
		}

		public override object Refuse { get { return "'Then may the waves swallow me alone. I’ll find my own way... if I don't drown in the songs first.'"; } }
		public override object Uncomplete { get { return "'The sea sings louder the longer we wait. Please, we must reach Trinsic Inn soon.'"; } }

		public OrwinDockswornQuest() : base()
		{
			AddObjective(new EscortObjective("Trinsic Inn"));
			AddReward(new BaseReward(typeof(MasterCello), "MasterCello – Enhances bardic music abilities"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("The sea's song fades... for now. Thank you. May this cello carry the melodies of peace, not torment.", null, 0x59B);
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

	public class OrwinDockswornEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(OrwinDockswornQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBBard());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public OrwinDockswornEscort() : base()
		{
			Name = "Orwin Docksworn";
			Title = "the Haunted Sailor-Poet";
			NameHue = 0x83F;
		}

		public OrwinDockswornEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(50, 50, 25);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83EA;
			HairItemID = 0x2049;
			HairHue = 1150;
			FacialHairItemID = 0x203B;
			FacialHairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new FancyShirt() { Hue = 0x482 });
			AddItem(new ElvenPants() { Hue = 0x497 });
			AddItem(new Cloak() { Hue = 0x59C });
			AddItem(new TricorneHat() { Hue = 0x59F });
			AddItem(new Boots() { Hue = 0x485 });
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
						"*Orwin hums softly, a haunting sea shanty* 'Oh the deep, dark sea, she’s calling me…'",
						"'The bard at Trinsic... he promised to help me. To write it down... so I can sleep.'",
						"*Orwin grips his harp tightly* 'Do you hear it too? The waves... the voices?'",
						"'I once sailed far beyond the known tides. I thought I found poetry... I found something else.'",
						"*He pauses, eyes distant* 'Some songs aren’t meant to be sung alone.'",
						"'We're close now, I can feel it. The verses are restless.'",
						"*Orwin shivers despite the warmth* 'Each step away from the sea, I breathe a little easier.'"
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
