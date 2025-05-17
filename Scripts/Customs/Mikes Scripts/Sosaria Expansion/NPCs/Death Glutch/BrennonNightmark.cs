using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilentStrikeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Strike"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Brennon Nightmark*, Guard Captain of Death Glutch, his armor scorched and bearing marks of battle.\n\n" +
                    "His gaze is cold, unwavering.\n\n" +
                    "“You’ve heard, haven’t you? Of the dignitaries? Marked for death by the shadows crawling out of the Malidor Witches Academy.”\n\n" +
                    "“An **Arcane Assassin** hides there, cloaked in magic, blades honed for a single strike. I trained these guards, I keep this town together. But that thing… it’s personal. One like it nearly took my life once, left me with this.”\n\n" +
                    "*He gestures to a jagged scar running down his left arm.*\n\n" +
                    "“I won’t let another fall. Not under my watch.”\n\n" +
                    "**Find the Arcane Assassin**, silence them, and return. Do this, and Death Glutch may yet breathe free another day.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then walk away. But if the assassin strikes again, know their blood is on your hands too.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive? Then the assassin is not. Or worse—they’ve found another target.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's done? The air feels lighter... for now.\n\n" +
                       "You’ve done more than end a life—you’ve given back time. Time for this town to prepare, to strengthen.\n\n" +
                       "*Take these.* **Explorer's Boots**—crafted for those who walk dangerous roads, like yours. May they carry you safely, as you’ve carried this burden for us.";
            }
        }

        public SilentStrikeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ArcaneAssassin), "Arcane Assassin", 1));
            AddReward(new BaseReward(typeof(ExplorersBoots), 1, "Explorer's Boots"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Strike'!");
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
	
	public class BrennonNightmark : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(SilentStrikeQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBWeaponSmith()); // Guard Captain providing basic weaponry and defense advice
		}

		[Constructable]
		public BrennonNightmark()
			: base("the Guard Captain", "Brennon Nightmark")
		{
		}

		public BrennonNightmark(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(95, 100, 50);

			Female = false;
			Body = 0x190; // Male
			Race = Race.Human;

			Hue = 33770; // Weathered skin tone
			HairItemID = 8251; // Short, rugged style
			HairHue = 1109; // Steel-gray
			FacialHairItemID = 8254; // Full beard
			FacialHairHue = 1109;
		}

		public override void InitOutfit()
		{
			AddItem(new PlateChest() { Hue = 1109, Name = "Scorched Guard Plate" }); // Charcoal black
			AddItem(new PlateLegs() { Hue = 1109, Name = "Marching Greaves" });
			AddItem(new PlateGloves() { Hue = 1109, Name = "Grip of Vengeance" });
			AddItem(new CloseHelm() { Hue = 1109, Name = "Nightmark Helm" });
			AddItem(new Cloak() { Hue = 1157, Name = "Cloak of the Glutch Watch" }); // Dark crimson
			AddItem(new Boots() { Hue = 1175, Name = "Iron-Tread Boots" });

			AddItem(new Longsword() { Hue = 0, Name = "Veteran's Edge" });

			Backpack backpack = new Backpack();
			backpack.Hue = 1150;
			backpack.Name = "Guard's Satchel";
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
