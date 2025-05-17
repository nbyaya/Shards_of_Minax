using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RustNeverSleepsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Rust Never Sleeps"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself face-to-face with *Dorrik Ironmuscle*, the grizzled Miner Foreman of Devil Guard.\n\n" +
                    "His armor is scorched, pitted with old acid burns, yet he stands unbowed, hammer in hand.\n\n" +
                    "“You smell that? Acid. *Again.* The shafts can't take much more, and neither can my crew.\n\n" +
                    "It’s that blasted **CorrodedRedSolen**. Thing’s been leaving trails that eat right through the beams—collapses are happening faster than we can patch 'em.\n\n" +
                    "I’ve faced beasts before, got the scars to prove it. But this one... it’s personal now. It won’t stop until the whole mine's rubble and we’re buried in it.\n\n" +
                    "**Find the CorrodedRedSolen and end it**. For my crew. For Devil Guard.”";
            }
        }

        public override object Refuse
        {
            get { return "Then you'd best stay outta the shafts. This Solen won't stop 'til we're all choked on dust and acid."; }
        }

        public override object Uncomplete
        {
            get { return "Still breathin'? Then that bug’s still crawlin'. My boys can't wait much longer."; }
        }

        public override object Complete
        {
            get
            {
                return
                    "It's dead? By the rocks, you did it!\n\n" +
                    "*Dorrik claps you on the shoulder, nearly knocking you over.*\n\n" +
                    "We can work again—*live* again, thanks to you.\n\n" +
                    "Take this: **NaptimeObliterator**. Used it once to knock a troll out cold. Figured I’d never need it again—but you? You’ve earned it.";
            }
        }

        public RustNeverSleepsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CorrodedRedSolen), "CorrodedRedSolen", 1));
            AddReward(new BaseReward(typeof(NaptimeObliterator), 1, "NaptimeObliterator"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Rust Never Sleeps'!");
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

    public class DorrikIronmuscle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RustNeverSleepsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public DorrikIronmuscle()
            : base("the Miner Foreman", "Dorrik Ironmuscle")
        {
        }

        public DorrikIronmuscle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered tan
            HairItemID = 0x2048; // Short hair
            HairHue = 1108; // Soot-black
            FacialHairItemID = 0x2041; // Beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1109, Name = "Foreman's Tempered Chestplate" }); // Dark steel
            AddItem(new StuddedGloves() { Hue = 2301, Name = "Acid-Singed Gauntlets" });
            AddItem(new StuddedLegs() { Hue = 1820, Name = "Ore-Stained Greaves" });
            AddItem(new LeatherGorget() { Hue = 1816, Name = "Miner’s Neckguard" });
            AddItem(new OrcHelm() { Hue = 2101, Name = "Rustbane Helm" });
            AddItem(new Boots() { Hue = 1102, Name = "Tunnelwalkers" });

            AddItem(new WarHammer() { Hue = 2401, Name = "Crewkeeper's Maul" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Foreman's Gearbag";
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
