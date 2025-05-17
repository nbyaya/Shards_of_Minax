using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RootTheInvaderQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Root the Invader"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Raggor Greenbough*, the stalwart hunter of Yew, standing amidst a tangle of corrupted roots near the forest edge.\n\n" +
                    "His rugged cloak rustles like leaves in the wind, and his eyes burn with fierce resolve.\n\n" +
                    "“These woods—*my woods*—are sick. There's rot creeping in from below, from that cursed place they call **Catastrophe**.”\n\n" +
                    "“It’s no ordinary blight. The **FungalBeast**—a living infestation—has spread its tendrils into my lodge, twisted trees, poisoned the air. If we don't cut it down, the whole forest will fall to spores and madness.”\n\n" +
                    "**Slay the FungalBeast**, burn its body, and return. Only then can the forest breathe freely again.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the woods wither. But know this—if we wait too long, nothing green will remain.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The rot still spreads? The lodge is lost, but Yew still stands—for now. But not for long if that beast still lives.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it… I can feel the forest sigh with relief.\n\n" +
                       "This tunic—*GroveboundTunic*—has been passed through hunters of Yew for generations. It’s woven with barkcloth and blessed by druids. May it shield you from shadow and root alike.";
            }
        }

        public RootTheInvaderQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FungalBeast), "FungalBeast", 1));
            AddReward(new BaseReward(typeof(GroveboundTunic), 1, "GroveboundTunic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Root the Invader'!");
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

    public class RaggorGreenbough : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RootTheInvaderQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner());
        }

        [Constructable]
        public RaggorGreenbough()
            : base("the Hunter of Yew", "Raggor Greenbough")
        {
        }

        public RaggorGreenbough(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 80);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1820; // Forest-brown
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1820;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2129, Name = "Mossbound Jerkin" }); // Earthy green
            AddItem(new LeatherLegs() { Hue = 2117, Name = "Thorn-Woven Greaves" });
            AddItem(new LeatherGloves() { Hue = 2111, Name = "Sap-Stained Gloves" });
            AddItem(new Cloak() { Hue = 2125, Name = "Hunter’s Shadecloak" });
            AddItem(new FeatheredHat() { Hue = 1824, Name = "Greenbough Cap" });
            AddItem(new Boots() { Hue = 2101, Name = "Rootwalkers" });

            AddItem(new CompositeBow() { Hue = 2109, Name = "Yew-Longbow" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Hunter’s Pack";
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
