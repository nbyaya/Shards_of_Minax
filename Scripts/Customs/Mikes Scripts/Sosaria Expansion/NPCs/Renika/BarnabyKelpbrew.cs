using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KemosCurseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Kemo’s Curse"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Barnaby Kelpbrew*, a sprightly alchemist whose robes reek of seaweed and salt.\n\n" +
                    "He fusses over a boiling flask, kelp strands wilting inside, his brow slick with sweat.\n\n" +
                    "“Blight the tides! These tremors—poisoning my brews! Our elixirs... tainted!”\n\n" +
                    "“GraniteKemo, they call it. A creature of stone and spite, nesting deep in Mountain Stronghold. Its heartstone pulses with unstable magic, twisting the earth beneath. I must have it! For the sake of every draught brewed here since my grandmother’s time!”\n\n" +
                    "**Slay the GraniteKemo**, bring me its heartstone, and I'll craft stability anew. Else Renika’s tides turn sour forever.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "No? Then mark me—the ground will rot, and the sea shall sour. I only hope another is brave enough to face Kemo.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no heartstone? My brews curdle as we speak! Hurry, before the tremors brew worse than storms!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ah, the heartstone—steady, solid, perfect! You've done what the sea could not: calmed the quake within.\n\n" +
                       "Take this: *ResolutionKeeper’s Sash*. May it ground you as you've grounded Renika’s lifeblood.";
            }
        }

        public KemosCurseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GraniteKemo), "GraniteKemo", 1));
            AddReward(new BaseReward(typeof(ResolutionKeepersSash), 1, "ResolutionKeeper’s Sash"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Kemo’s Curse'!");
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

    public class BarnabyKelpbrew : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(KemosCurseQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public BarnabyKelpbrew()
            : base("the Kelp Alchemist", "Barnaby Kelpbrew")
        {
        }

        public BarnabyKelpbrew(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1005; // Slightly pale sea-worn complexion
            HairItemID = 0x203B; // Short Hair
            HairHue = 1150; // Seaweed green
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1150; // Matching green
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 2101, Name = "Tidewoven Robe" }); // Deep sea blue
            AddItem(new WizardsHat() { Hue = 2105, Name = "Kelpbrew’s Cap" }); // Mossy green hat
            AddItem(new Sandals() { Hue = 1821, Name = "Salt-Crusted Sandals" }); // Sandy hue
            AddItem(new BodySash() { Hue = 2501, Name = "Foamline Sash" }); // Pale aqua, symbolizing seafoam
            AddItem(new HalfApron() { Hue = 2118, Name = "Brine-Stained Apron" }); // Sea-tarnished apron
            AddItem(new Bottle() { Name = "Vial of Kelp Draught" }); // For flavor
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
