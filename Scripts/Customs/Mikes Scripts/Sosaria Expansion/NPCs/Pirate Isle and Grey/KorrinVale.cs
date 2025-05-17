using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class ShatteredGloryQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		public override object Title { get { return "Shattered Glory"; } }

		public override object Description
		{
			get
			{
				return
					"You face *Magistrate Korrin Vale*, stern judge of Grey’s portside court.\n\n" +
					"He grips a heavy gavel, its surface cracked and pulsing faintly with ghostly light.\n\n" +
					"“There was once a man—a jurist of great repute—who judged all fairly, but pride festered in him. When the Exodus rose, he made dark pacts to protect the law, and in so doing, shattered his soul.”\n\n" +
					"**The ShatteredMajesty** now stalks the ruins of our courthouse, rendering judgment upon the living, shackled to its own broken oaths.”\n\n" +
					"“I cannot face him. But you—perhaps you can slay what he has become and end his torment.”\n\n" +
					"**Slay the ShatteredMajesty** in the depths of Exodus Dungeon, and return peace to Grey’s legacy of law.";
			}
		}

		public override object Refuse
		{
			get { return "Then may his wails never reach your ears, for I fear they shall reach mine every night until his gavel falls silent."; }
		}

		public override object Uncomplete
		{
			get { return "Still he judges? The shadows lengthen, and I hear his voice in the silence of the docks."; }
		}

		public override object Complete
		{
			get
			{
				return "The law has been upheld—not in parchment or court, but in steel and spirit.\n\n" +
					   "Take this: *DragonClaw*. A relic as fierce as justice itself. May it strike as true for you as your resolve has today.";
			}
		}

		public ShatteredGloryQuest() : base()
		{
			AddObjective(new SlayObjective(typeof(ShatteredMajesty), "ShatteredMajesty", 1));
			AddReward(new BaseReward(typeof(DragonClaw), 1, "DragonClaw"));
		}

		public override void OnCompleted()
		{
			Owner.SendMessage(0x23, "You've completed 'Shattered Glory'!");
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


	public class KorrinVale : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(ShatteredGloryQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBProvisioner()); // As a Magistrate, he oversees general supplies, especially legal tools and parchments.
		}

		[Constructable]
		public KorrinVale()
			: base("the City Judge", "Korrin Vale")
		{
		}

		public KorrinVale(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(80, 70, 80);

			Female = false;
			Body = 0x190;
			Race = Race.Human;

			Hue = 1055; // Weathered bronze skin tone.
			HairItemID = 0x203B; // Long Hair
			HairHue = 1102; // Shadow gray
			FacialHairItemID = 0x2041; // Short Beard
			FacialHairHue = 1102;
		}

		public override void InitOutfit()
		{
			AddItem(new Robe() { Hue = 1157, Name = "Judicator's Robe" }); // Deep navy-blue
			AddItem(new LeatherGloves() { Hue = 2115, Name = "Grasp of Law" }); // Dark iron
			AddItem(new HalfApron() { Hue = 1153, Name = "Magistrate's Sash" }); // Midnight blue
			AddItem(new Boots() { Hue = 1109, Name = "Tread of Verdict" }); // Ash black
			AddItem(new FeatheredHat() { Hue = 1157, Name = "Plumed Judgement" }); // Matching his robe
			AddItem(new Scepter() { Hue = 2101, Name = "Gavel of Binding" }); // His enchanted gavel, faintly glowing

			Backpack backpack = new Backpack();
			backpack.Hue = 1150;
			backpack.Name = "Judicial Satchel";
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
