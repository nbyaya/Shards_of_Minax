using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScaleOfDecayQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Scale of Decay"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Gwynor Swiftleaf*, a seasoned tracker near Yew, his eyes sharp beneath a hood woven from forest shadows.\n\n" +
                    "“I’ve walked these woods for years, and never have I seen such sickness seep from the earth. A beast, *DecayingLizard*, has fouled the springs with its breath—its scales rot, and the rot spreads.”\n\n" +
                    "“We spotted it at dusk, near the old well. Eyes like dying embers. It slips between shadow and muck. But I swear on the trees—**I won’t let Yew drink poison.**”\n\n" +
                    "**Slay the DecayingLizard** before our waters run black. The forest remembers those who guard it.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the woods forgive us. But I fear for the wells, and those who drink unknowing.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it slithers? Each dawn brings darker pools. Even the birds will not drink now.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast falls... and the forest breathes again.\n\n" +
                       "You’ve cleansed more than a spring—you’ve given hope to the roots. Take this: *SashOfTheMoonBound*. May it guide your path as you’ve safeguarded ours.";
            }
        }

        public ScaleOfDecayQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DecayingLizard), "DecayingLizard", 1));
            AddReward(new BaseReward(typeof(SashOfTheMoonBound), 1, "SashOfTheMoonBound"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Scale of Decay'!");
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

    public class GwynorSwiftleaf : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ScaleOfDecayQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer());
        }

        [Constructable]
        public GwynorSwiftleaf()
            : base("the Forest Tracker", "Gwynor Swiftleaf")
        {
        }

        public GwynorSwiftleaf(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Light forest tone
            HairItemID = Race.RandomHair(this);
            HairHue = 2207; // Mossy green-tinted brown
            FacialHairItemID = 0x204B; // Short Beard
            FacialHairHue = 2207;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1825, Name = "Tracker's Tunic" }); // Deep forest green
            AddItem(new StuddedLegs() { Hue = 1837, Name = "Thorn-Guarded Leggings" }); // Bark-brown
            AddItem(new LeatherGloves() { Hue = 1839, Name = "Sap-Stained Gloves" });
            AddItem(new LeatherCap() { Hue = 1822, Name = "Hood of Whispering Leaves" });
            AddItem(new Cloak() { Hue = 1820, Name = "Cloak of Shadowed Trails" });
            AddItem(new Boots() { Hue = 1833, Name = "Silent Step Boots" });

            AddItem(new CompositeBow() { Hue = 2101, Name = "Moonbound Longbow" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Tracker's Pack";
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
