using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DevilsDenQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Devil’s Den"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Roderic Quillmark*, the meticulous scribe of Dawn, seated amidst torn scrolls and scattered ink pots.\n\n" +
                    "His ink-stained fingers tremble as he gestures to a half-burned manuscript.\n\n" +
                    "“I toil for clarity, for truth. Yet now, truth itself is tainted. A *daemonic ferret* haunts my scriptorium, stealing inks and gnawing at knowledge itself!”\n\n" +
                    "“It slinks from the depths—*from Doom*, they say. Left stains on my works... stains that whisper. Flames follow its paws. It laughs as it burns my thoughts.”\n\n" +
                    "**Slay the Daemonic Ferret** that desecrates the written word, and restore sanity to the scribes of Dawn.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I am doomed to scribe in fear, every word a flicker from the infernal shadow.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it scurries? The whispers grow louder, and my ink runs dry...";
            }
        }

        public override object Complete
        {
            get
            {
                return "The beast is vanquished? The whispers fade... ink flows pure once more.\n\n" +
                       "**Take this—*BloodbarkCleaver*.** A token of gratitude, hewn from trees that bled for knowledge lost. May it guard your mind as it did mine.";
            }
        }

        public DevilsDenQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DaemonicFerret), "Daemonic Ferret", 1));
            AddReward(new BaseReward(typeof(BloodbarkCleaver), 1, "BloodbarkCleaver"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Devil’s Den'!");
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

    public class RodericQuillmark : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DevilsDenQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public RodericQuillmark()
            : base("the Ink-Warded Scribe", "Roderic Quillmark")
        {
        }

        public RodericQuillmark(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 80, 90);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Pale ink-touched skin tone
            HairItemID = 0x2048; // Short Hair
            HairHue = 1150; // Midnight blue
            FacialHairItemID = 0x2041; // Trimmed beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1175, Name = "Inkthread Tunic" }); // Deep Indigo
            AddItem(new ElvenPants() { Hue = 2101, Name = "Scroll-Keeper's Trousers" }); // Faded parchment
            AddItem(new BodySash() { Hue = 1153, Name = "Glyphbound Sash" }); // Arcane blue
            AddItem(new Sandals() { Hue = 1109, Name = "Dustwalkers" }); // Ash gray
            AddItem(new FeatheredHat() { Hue = 1170, Name = "Quillcrest Hat" }); // Dark violet, feather tipped in crimson
            AddItem(new SpellWeaversWand() { Hue = 0, Name = "Inkweaver's Rod" }); // Plain, but infused with subtle sparkles
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
