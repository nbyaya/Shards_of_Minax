using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class SnakeOilQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		public override object Title { get { return "Snake Oil"; } }

		public override object Description
		{
			get
			{
				return "Nyssa Pixietracker, a seasoned ranger, greets you with a nod.\n\n" +
					   "\"The miners fear something... serpentine. A knight, twisted by venom, whose oath rots in the dark. I tracked him—followed shadows and whispers.\"\n\n" +
					   "\"He was once part of a tribe I knew... Now? He wields poison like a prayer, and that banner he carries—if it touches our miners, death will follow.\"\n\n" +
					   "**Slay the InsaneOphidianKnight** and return with his venomous banner. Before more are lost.\"";
			}
		}

		public override object Refuse => "Then the venom spreads... and we count the cost in graves.";
		public override object Uncomplete => "The knight still roams? His poison thickens in the air. Hurry.";
		public override object Complete => "You’ve done it. The banner… it reeks of sorrow. But it will harm no one else. Take this—it’s more than steel. It's the strength to endure.";

		public SnakeOilQuest() : base()
		{
			AddObjective(new SlayObjective(typeof(InsaneOphidianKnight), "Insane Ophidian Knight", 1));
			AddReward(new BaseReward(typeof(LatticeOfTheFallenLegion), 1, "LatticeOfTheFallenLegion"));
		}

		public override void OnCompleted()
		{
			Owner.SendMessage(0x23, "You've completed 'Snake Oil'!");
			Owner.PlaySound(CompleteSound);
		}
	}

	public class NyssaPixietracker : MondainQuester
	{
		public override Type[] Quests => new Type[] { typeof(SnakeOilQuest) };
		public override bool IsActiveVendor => true;
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBRanger());
		}

		[Constructable]
		public NyssaPixietracker()
			: base("the Ranger", "Nyssa Pixietracker")
		{
		}

		public NyssaPixietracker(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(90, 80, 50);

			Female = true;
			Body = 0x191;
			Race = Race.Human;

			Hue = Race.RandomSkinHue();
			HairItemID = Race.RandomHair(this);
			HairHue = 1812; // Dark brown
		}

		public override void InitOutfit()
		{
			AddItem(new LeatherNinjaHood() { Hue = 2117, Name = "Tracker’s Shade" });
			AddItem(new StuddedHiroSode() { Hue = 2105, Name = "Bracers of the Verdant Watch" });
			AddItem(new LeatherBustierArms() { Hue = 2113, Name = "Windswept Ranger’s Vest" });
			AddItem(new FurSarong() { Hue = 1820, Name = "Hunter’s Trail Wrap" });
			AddItem(new FurBoots() { Hue = 1815, Name = "Silentstep Fur Boots" });
			AddItem(new BodySash() { Hue = 1821, Name = "Banner of the Lost Path" });
			AddItem(new RangersCrossbow() { Hue = 2407, Name = "Thornwhisper" });

			Backpack backpack = new Backpack();
			backpack.Hue = 2101;
			backpack.Name = "Ranger’s Pack";
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