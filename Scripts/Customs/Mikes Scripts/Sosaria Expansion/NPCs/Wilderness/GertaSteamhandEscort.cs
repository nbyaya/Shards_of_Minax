using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class GertaSteamhandQuest : BaseQuest
	{
		public override object Title { get { return "Schematics in the Storm"; } }

		public override object Description
		{
			get
			{
				return 
					"Oh! Sparks and soot! You there! I’m Gerta Steamhand, apprentice engineer of East Montor’s Tinkerers’ Guild. This storm's gone and fried my navigation gyros! I've urgent schematics buzzing in my pack—unstable mining tech, highly experimental—and they need to reach Devil Guard before things... explode. You’ll guide me, yes? But careful now, my gear’s humming like a hive of angry wasps.";
			}
		}

		public override object Refuse { get { return "You’d leave me to stumble through these mountains alone, with lightning crackling at my back and doom strapped to my shoulders? Have a heart!"; } }
		public override object Uncomplete { get { return "My pack’s heating up—please, we’ve not much time! The mountains stir, and the schematics mustn’t fall into the wrong hands... or rocks!"; } }

		public GertaSteamhandQuest() : base()
		{
			AddObjective(new EscortObjective("Devil Guard Inn"));
			AddReward(new BaseReward(typeof(WeaponOil), "WeaponOil"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("Thank you, thank you! Here, take this WeaponOil—it’ll give your weapon a bite as sharp as my tongue! I owe you, truly.", null, 0x59B);
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

	public class GertaSteamhandEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(GertaSteamhandQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBTinker(this));
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public GertaSteamhandEscort() : base()
		{
			Name = "Gerta Steamhand";
			Title = "the Tinkerer";
			NameHue = 0x83F;
		}

		public GertaSteamhandEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(45, 55, 35);
			Female = true;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x83EA;
			HairItemID = 0x203B;
			HairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new ClothNinjaHood() { Hue = 0x430 });
			AddItem(new ElvenShirt() { Hue = 0x455 });
			AddItem(new LeatherShorts() { Hue = 0x1BB });
			AddItem(new HalfApron() { Hue = 0x497 });
			AddItem(new FurBoots() { Hue = 0x1C2 });
			AddItem(new LeatherGloves() { Hue = 0x3A6 });
			AddItem(new GearLauncher());
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
						"*Gerta fiddles nervously with her humming pack* 'Easy now, just a little more juice, not too much... not like last time.'",
						"'Did I mention the schematics are... slightly volatile? Oh, I did? Well, they’re more volatile now.'",
						"*She peers into the misty trail* 'Devil Guard’s just over the next ridge... or is it two? Please say it’s just one.'",
						"*Her pack emits a sharp crackle* 'That wasn’t me... was it?'",
						"'Do you smell something burning? Oh wait, that’s just my nerves.'",
						"'If we make it, I’ll bake you a gear-shaped pie. If we don’t... well, let’s not think about that.'"
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