using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TemporalAnnihilationQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Temporal Annihilation"; } }

        public override object Description
        {
            get
            {
                return
                    "Lydia Timebender stands within Castle British’s Hall of Echoes, her robes shimmering faintly, as if touched by light from a different era.\n\n" +
                    "Her voice, calm but urgent, echoes oddly, as if layered through time itself.\n\n" +
                    "“You can feel it, can’t you? The distortion? Something has entered **Vault 44**, something *wrong*—a fracture incarnate.”\n\n" +
                    "“The **Anachronaut**, a creature not of now, not of ever. If it completes its loop, it will **shatter the Vault’s time anchor**, and with it, reality may unravel beneath Britain.”\n\n" +
                    "“I apprenticed under Lady Isobel, sealing minor rifts outside these walls. But this... this is beyond me alone. You must enter, and you must end it.”\n\n" +
                    "**Slay the Anachronaut** before time frays beyond repair.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If you won’t intervene, then the thread of now may snap. But I will watch, and hope the damage can yet be contained.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Vault’s pulses grow wilder. Have you not felt the hours twist? The Anachronaut still thrives in the chaos.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve returned... and time holds steady.\n\n" +
                       "The Anachronaut is no more, and with it, the Vault’s fracture begins to mend.\n\n" +
                       "Take this: *BackstreetBolt*—a relic infused with threads of what might have been. Use it well, should time ever betray us again.";
            }
        }

        public TemporalAnnihilationQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Anachronaut), "Anachronaut", 1));
            AddReward(new BaseReward(typeof(BackstreetBolt), 1, "BackstreetBolt"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Temporal Annihilation'!");
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

    public class LydiaTimebender : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TemporalAnnihilationQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTailor()); // Closest vendor type to a Chronomancer
        }

        [Constructable]
        public LydiaTimebender()
            : base("the Chronomancer", "Lydia Timebender")
        {
        }

        public LydiaTimebender(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Moonlit silver
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1150, Name = "Chronoweave Robe" }); // Ethereal blue
            AddItem(new WizardsHat() { Hue = 1175, Name = "Temporal Hood" }); // Cosmic indigo
            AddItem(new Sandals() { Hue = 1170, Name = "Pathless Sandals" }); // Shadowy violet
            AddItem(new Cloak() { Hue = 1154, Name = "Echoing Cloak" }); // Faintly shimmering gray

            AddItem(new SpellWeaversWand() { Hue = 1152, Name = "Threadbinder" }); // Pale blue wand with a time sigil

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Chronomancer's Pack";
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
