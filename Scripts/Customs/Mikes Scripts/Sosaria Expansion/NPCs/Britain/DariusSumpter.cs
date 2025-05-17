using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SealTheBreachQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Seal the Breach"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Darius Sumpter*, Vault Curator of Castle British, surrounded by scrolls and glowing containment diagrams.\n\n" +
                    "His robes flicker faintly with stored arcane energy, and he peers up, eyes haunted but sharp.\n\n" +
                    "“The Preservation Vault... it’s compromised. The *ContainmentSpill*—it’s not just a leak, it’s sentient. The vault’s integrity is below 60%, and artifacts preserved for centuries are warping under its influence.”\n\n" +
                    "“This isn't just rot or corruption—it’s a cascading breakdown of everything we’ve stored, everything we’ve protected from the Collapse.”\n\n" +
                    "“If that entity isn't destroyed, the seal will fail completely. Toxic echoes, arcane contamination... the damage will spread.”\n\n" +
                    "**Slay the ContainmentSpill** and let me reestablish the security seal before the Vault becomes a tomb of volatile history.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the breach widens, and time unravels our efforts. Pray the Vault holds a little longer...";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The breach grows! Each moment lost allows the Spill to twist what should never change!";
            }
        }

        public override object Complete
        {
            get
            {
                return "The Vault stabilizes... the seal hums once more. You’ve done it.\n\n" +
                       "Take this: the *Circuit of the Echoing Will*. Crafted to retain essence beyond the breach—wear it, and resist the unraveling of the world’s order.";
            }
        }

        public SealTheBreachQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ContainmentSpill), "ContainmentSpill", 1));
            AddReward(new BaseReward(typeof(CircuitOfTheEchoingWill), 1, "Circuit of the Echoing Will"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Seal the Breach'!");
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

	public class DariusSumpter : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(SealTheBreachQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBTinker(this)); // As a Vault Curator, he deals in arcane mechanisms and containment devices.
		}

		[Constructable]
		public DariusSumpter()
			: base("the Vault Curator", "Darius Sumpter")
		{
		}

		public DariusSumpter(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(75, 80, 90);

			Female = false;
			Body = 0x190; // Male
			Race = Race.Human;

			Hue = 1002; // Pale complexion from working underground.
			HairItemID = Race.RandomHair(this);
			HairHue = 1150; // Silver-blue
			FacialHairItemID = Race.RandomFacialHair(this);
			FacialHairHue = 1150;
		}

		public override void InitOutfit()
		{
			AddItem(new HoodedShroudOfShadows() { Hue = 1153, Name = "Curator's Shroud" }); // Deep midnight blue
			AddItem(new LeatherGloves() { Hue = 1151, Name = "Seal-Ward Gloves" });
			AddItem(new Boots() { Hue = 1109, Name = "Echo-Walkers" });
			AddItem(new BodySash() { Hue = 1160, Name = "Glyphbound Sash" });

			AddItem(new ArtificerWand() { Hue = 1175, Name = "Containment Rod" });

			Backpack backpack = new Backpack();
			backpack.Hue = 1150;
			backpack.Name = "Curator's Pack";
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
