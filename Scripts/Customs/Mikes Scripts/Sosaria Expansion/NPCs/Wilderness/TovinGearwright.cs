using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class TovinQuest : BaseQuest
	{
		public override object Title { get { return "The Lightning Path"; } }

		public override object Description
		{
			get
			{
				return 
					"*Tovin's eyes flicker with excitement as he adjusts a sparking device on his wrist.* 'You're just in time! My name’s Tovin Gearwright, inventor extraordinaire! I’ve created something revolutionary—a bow that harnesses lightning! But the guild in East Montor must see it... before others do. There are people who’d rather see me silenced. Will you guide me safely to East Montor? Together, we might change the world—or at least, give it a good shock!'";
			}
		}

		public override object Refuse { get { return "'No? But... who else will protect the future of ranged combat?! This is madness! I’ll go alone, then! And probably explode!';"; } }
		public override object Uncomplete { get { return "'Still so far? My prototype’s getting unstable... It needs a charge boost soon or it might— *sparks wildly*—Never mind, never mind, let’s just keep moving!"; } }

		public TovinQuest() : base()
		{
			AddObjective(new EscortObjective("the town of East Montor"));
			AddReward(new BaseReward(typeof(GenjiBow), "GenjiBow – The Stormstring Bow"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("You’ve done it! East Montor will never forget this day, and neither will I! Take the GenjiBow, a gift from one innovator to another. May your arrows fly like lightning!", null, 0x59B);
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

	public class TovinGearwright : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(TovinQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBTinker(this)); // He's an inventor, Tinker fits best.
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public TovinGearwright() : base()
		{
			Name = "Tovin Gearwright";
			Title = "the Aspiring Inventor";
			NameHue = 0x489;
		}

		public TovinGearwright(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 55, 40);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83E8;
			HairItemID = 0x2048;
			HairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new FancyShirt() { Hue = 0x47F });
			AddItem(new ElvenPants() { Hue = 0x46F });
			AddItem(new LeatherGloves() { Hue = 0x6E4 });
			AddItem(new Cloak() { Hue = 0x501 });
			AddItem(new Boots() { Hue = 0x497 });
			AddItem(new BodySash() { Hue = 0x505 });
			AddItem(new GearLauncher()); // Custom weapon/tool, fits his tinkering nature.
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
						"*Tovin fiddles with a sparking device* 'Easy now... I’ve calibrated the charge! Probably.'",
						"'Do you smell that? That’s ozone! Or burning cloth... either way, let’s keep moving!'",
						"'Once we reach East Montor, they'll see! They'll ALL see! *laughs maniacally then coughs*'",
						"*Tovin pulls out a tiny notebook* 'Note: Insulate handle better next time... and maybe the trigger.'",
						"'I’m pretty sure that noise wasn’t normal. Was that the prototype? Or something following us?'",
						"*He adjusts a lens on his goggles* 'You know, lightning’s just misunderstood magic with flair.'",
						"'Do you think the guild will let me name it? I’m thinking... Stormstring, or maybe Arcstrike!'"
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

	// Custom Reward Item
	public class GenjiBow : CompositeBow
	{
		public override int InitMinHits => 60;
		public override int InitMaxHits => 120;

		[Constructable]
		public GenjiBow() : base()
		{
			Name = "GenjiBow";
			Hue = 0x481;
			Attributes.WeaponSpeed = 20;
			Attributes.SpellChanneling = 1;
			WeaponAttributes.HitLightning = 40;
		}

		public GenjiBow(Serial serial) : base(serial) { }

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
