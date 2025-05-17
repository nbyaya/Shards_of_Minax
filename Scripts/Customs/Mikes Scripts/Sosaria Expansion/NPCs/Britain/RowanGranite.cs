using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GolemInTimeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Golem in Time"; } }

        public override object Description
        {
            get
            {
                return
                    "Rowan Granite, Runecaster of Castle British, stands before a circle of softly glowing runestones.\n\n" +
                    "His hands are ink-stained, and strange symbols float faintly in the air around him, dissolving as he speaks.\n\n" +
                    "“Time falters in the Vault. The **ChronoColossus**—a relic of the Planar Imperium—has stirred again. Each step it takes frays the bindings we laid to seal Vault 44. If it reaches the heart of the vault, the wards will fail, and **what lies beyond time will breach**.”\n\n" +
                    "“I am bound here, maintaining what little control remains. You must enter the vault, follow the rhythm of its runes, and **destroy the ChronoColossus** before the next lunar shift. If you succeed, I will grant you something woven from the void itself.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the runes hold... though I fear they will not. The ticking echoes louder now, and time slips through our fingers.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The ChronoColossus still marches? I can feel it. The runes flicker and the vault groans with its passing. There is little time left.";
            }
        }

        public override object Complete
        {
            get
            {
                return
                    "It is done? The Colossus lies still... then the vault breathes again. You’ve not just slain a beast, you’ve saved **the rhythm of Sosaria** from unraveling.\n\n" +
                    "Take this, the *ShroudOfTheHollowSky*—a fragment of time itself, for one who has faced the clockwork of fate.";
            }
        }

        public GolemInTimeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ChronoColossus), "ChronoColossus", 1));
            AddReward(new BaseReward(typeof(ShroudOfTheHollowSky), 1, "ShroudOfTheHollowSky"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Golem in Time'!");
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

    public class RowanGranite : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GolemInTimeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic()); // Best fit for a Runecaster
        }

        [Constructable]
        public RowanGranite()
            : base("the Runecaster", "Rowan Granite")
        {
        }

        public RowanGranite(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2047; // Long hair
            HairHue = 1150; // Ghostly silver
            FacialHairItemID = 0x203F; // Medium beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1153, Name = "Chrono-Woven Robe" }); // Deep blue, time-warped hue
            AddItem(new BodySash() { Hue = 1150, Name = "Runebinder’s Sash" });
            AddItem(new WizardsHat() { Hue = 1153, Name = "Hat of Echoing Runes" });
            AddItem(new Sandals() { Hue = 1109, Name = "Vault-Walker’s Sandals" });

            AddItem(new SpellWeaversWand() { Hue = 1175, Name = "Temporal Focus Wand" }); // Unique weapon appearance

            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Rune-Inscribed Pack";
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

