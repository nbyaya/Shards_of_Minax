using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class FrozenPackQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		public override object Title { get { return "Howl of the Frozen Pack"; } }

		public override object Description
		{
			get
			{
				return
					"You approach *Lyanna Icefang*, the Kennel Mistress of Mountain Crest.\n\n" +
					"Clad in frost-touched leathers, her gaze burns with restrained fury. Around her, war-dogs growl low, sensing their mistress’s tension.\n\n" +
					"\"One of ours turned prey. Not to any ordinary beast, no. A **Frostbite Hound** – cursed, twisted by Ice Cavern’s magic. My war-dogs—raised from pups, loyal to their last breath—are dead, or worse, missing. That hound mocks me, hunting my pack like game in the snow.\"\n\n" +
					"\"You—hunter, warrior, wanderer—I care not your title. Find that thing. **End it**. Bring me peace, or don’t come back.\"\n\n" +
					"**Slay the Frostbite Hound** that stalks Ice Cavern, and let Mountain Crest’s howls rise proud again.";
			}
		}

		public override object Refuse
		{
			get
			{
				return "Then leave me to mourn alone. But know this: every night that thing lives, another howl is lost to the frost.";
			}
		}

		public override object Uncomplete
		{
			get
			{
				return "Still it hunts? Still it feasts on my kin? Find it. Kill it. Or our pack’s blood is on your hands too.";
			}
		}

		public override object Complete
		{
			get
			{
				return "The howls I hear now... they’re proud, not pained.\n\n" +
					   "You’ve done what I could not. For that, you earn more than coin. **You earn our trust**.\n\n" +
					   "Take this, the *AstartesBattlePlate*. Worn by my clan in wars long past—may it serve you as well as you’ve served me.";
			}
		}

		public FrozenPackQuest() : base()
		{
			AddObjective(new SlayObjective(typeof(FrostbiteHound), "Frostbite Hound", 1));
			AddReward(new BaseReward(typeof(AstartesBattlePlate), 1, "AstartesBattlePlate"));
		}

		public override void OnCompleted()
		{
			Owner.SendMessage(0x23, "You've completed 'Howl of the Frozen Pack'!");
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

	public class LyannaIcefang : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(FrozenPackQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBButcher());
		}

		[Constructable]
		public LyannaIcefang()
			: base("the Kennel Mistress", "Lyanna Icefang")
		{
		}

		public LyannaIcefang(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(90, 100, 40);

			Female = true;
			Body = 0x191; // Female
			Race = Race.Human;

			Hue = Race.RandomSkinHue();
			HairItemID = Race.RandomHair(this);
			HairHue = 1150; // Snow-white
		}

		public override void InitOutfit()
		{
			AddItem(new LeatherHiroSode() { Hue = 1152, Name = "Icefang Pauldrons" });
			AddItem(new LeatherBustierArms() { Hue = 1153, Name = "Frosthide Vest" });
			AddItem(new LeatherSkirt() { Hue = 2101, Name = "Snowbound Skirt" });
			AddItem(new FurBoots() { Hue = 1109, Name = "Wolfstep Boots" });
			AddItem(new BearMask() { Hue = 1150, Name = "White Fang Mask" });
			AddItem(new ShepherdsCrook() { Hue = 1154, Name = "Icefang's Crook" });
			AddItem(new Cloak() { Hue = 1157, Name = "Wolfsbane Cloak" });
			AddItem(new BodySash() { Hue = 2105, Name = "Pack Mistress Sash" });

			Backpack backpack = new Backpack();
			backpack.Hue = 1152;
			backpack.Name = "Kennel Satchel";
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
