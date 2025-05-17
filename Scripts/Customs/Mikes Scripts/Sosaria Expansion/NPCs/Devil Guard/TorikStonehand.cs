using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class PlugTheDrillersDoomQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		public override object Title { get { return "Plug the Driller’s Doom"; } }

		public override object Description
		{
			get
			{
				return
					"Torik Stonehand, the Tunnel Engineer of Devil Guard, grips a twisted, soot-darkened blueprint, torn along its edges.\n\n" +
					"“You hear that? No? Lucky you.”\n\n" +
					"“That thing—they call it **BurrowerKeppto**—has been tunneling under our feet. Just collapsed the northern drift. Cost me weeks, good men, and the last of our iron supports.”\n\n" +
					"“I grew up near a mine like this, lost my left hand to one of those damned burrowers when I was just a boy. Thought I left them behind. Thought wrong.”\n\n" +
					"“Plug it. Slay it. I don’t care how, but **stop Keppto** before Devil Guard itself starts sinking.”\n\n" +
					"“Bring me proof, and I’ll give you something worth your while—**the SilverWreathOfEddasVision**. Some say it wards against tremors... and worse.”";
			}
		}

		public override object Refuse
		{
			get
			{
				return "Fine. Stay clear of the tunnels then, but don’t say I didn’t warn you when you hear the ground crack beneath your feet.";
			}
		}

		public override object Uncomplete
		{
			get
			{
				return "Still alive, are you? Then Keppto must still be digging. It’ll bring the whole damn mountain down if we don’t act.";
			}
		}

		public override object Complete
		{
			get
			{
				return "You did it? Ha! That’s one less nightmare gnawing at these rocks.\n\n" +
					   "Take this. It’s old, but it’s seen me through cave-ins and worse. **The SilverWreath** isn’t just silver, you know—it’s a piece of prophecy. Wear it, and may your path be steady.";
			}
		}

		public PlugTheDrillersDoomQuest() : base()
		{
			AddObjective(new SlayObjective(typeof(BurrowerKeppto), "BurrowerKeppto", 1));
			AddReward(new BaseReward(typeof(SilverWreathOfEddasVision), 1, "SilverWreathOfEddasVision"));
		}

		public override void OnCompleted()
		{
			Owner.SendMessage(0x23, "You've completed 'Plug the Driller’s Doom'!");
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
		
		public class TorikStonehand : MondainQuester
		{
			public override Type[] Quests { get { return new Type[] { typeof(PlugTheDrillersDoomQuest) }; } }

			public override bool IsActiveVendor { get { return true; } }
			public override bool UsesRandomisedStock => true;

			public override void InitSBInfo()
			{
				m_SBInfos.Add(new SBMiner()); // Miner fits his Tunnel Engineer role
			}

			[Constructable]
			public TorikStonehand()
				: base("the Tunnel Engineer", "Torik Stonehand")
			{
			}

			public TorikStonehand(Serial serial) : base(serial) { }

			public override void InitBody()
			{
				InitStats(90, 85, 50);

				Female = false;
				Body = 0x190; // Male
				Race = Race.Human;

				Hue = 2001; // Weathered miner's tan
				HairItemID = 0x203B; // Long hair
				HairHue = 1107; // Ash-black
				FacialHairItemID = 0x2041; // Long beard
				FacialHairHue = 1107;
			}

			public override void InitOutfit()
			{
				AddItem(new LeatherDo() { Hue = 2419, Name = "Engineer's Mantle" }); // Soot-stained dark brown
				AddItem(new StuddedLegs() { Hue = 1824, Name = "Tunnel-Sealed Greaves" }); // Dusty grey
				AddItem(new PlateGloves() { Hue = 1157, Name = "Stonehand Gauntlets" }); // One glove covers his real hand, other is a metal prosthetic
				AddItem(new LeatherCap() { Hue = 1175, Name = "Miner's Crest" }); // Deep blue miner's cap
				AddItem(new HalfApron() { Hue = 2201, Name = "Blueprint Holder's Apron" }); // Worn reddish brown
				AddItem(new Boots() { Hue = 2101, Name = "Dust-Treader Boots" }); // Scuffed black

				AddItem(new HammerPick() { Hue = 1109, Name = "Tunnelbreaker" });

				Backpack backpack = new Backpack();
				backpack.Hue = 1175;
				backpack.Name = "Engineer’s Pack";
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
}