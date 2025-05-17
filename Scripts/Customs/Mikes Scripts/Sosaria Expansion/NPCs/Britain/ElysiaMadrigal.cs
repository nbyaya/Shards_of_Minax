using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilenceTheBansheeChorus : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silence the Banshee Chorus"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Elysia Madrigal*, Choir Master of Castle British.\n\n" +
                    "She adjusts the folds of her twilight-blue robe, fingers trembling slightly as a faint melody lingers on her breath.\n\n" +
                    "“Have you heard it? The lament? The *ChoirWraith* sings again from within **Vault 44’s grand amphitheater**. It was once a singer of celestial hymns… but now its song twists melody into sickness. My musicians hear it in their dreams, and their notes falter.”\n\n" +
                    "“I trained at the same cathedral as it. I *know* its voice. I *feel* it in the walls of this castle. If the ChoirWraith’s dissonant hymn continues, our music will die—or worse, become something *wrong*.”\n\n" +
                    "**Slay the ChoirWraith** before its corrupted chorus consumes the soul of Castle British.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your ears remain deaf to the song. But the rest of us will suffer the melody in silence.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The song continues. My students falter, their voices strained... I fear the ChoirWraith’s lament is growing stronger.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s over... The echoes fade. For the first time in weeks, I hear *silence*. \n\n" +
                       "You’ve not just saved us—you’ve given us our voices back.\n\n" +
                       "Take this: *MercysEdge.* Forged for those who face darkness with clarity, as you have. Let its edge guide you as surely as you’ve guided us to peace.";
            }
        }

        public SilenceTheBansheeChorus() : base()
        {
            AddObjective(new SlayObjective(typeof(ChoirWraith), "ChoirWraith", 1));
            AddReward(new BaseReward(typeof(MercysEdge), 1, "MercysEdge"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silence the Banshee Chorus'!");
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

    public class ElysiaMadrigal : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilenceTheBansheeChorus) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel()); // As Choir Master, she's a bard-type vendor.
        }

        [Constructable]
        public ElysiaMadrigal()
            : base("the Choir Master", "Elysia Madrigal")
        {
        }

        public ElysiaMadrigal(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Pale tone
            HairItemID = 0x2049; // Long hair
            HairHue = 1153; // Silvery white
            NameHue = 1154; // Unique name hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1150, Name = "Twilight Chorus Robe" }); // Deep blue robe
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Silversong Cap" }); // Silver feathered hat
            AddItem(new Sandals() { Hue = 1154, Name = "Choral Sandals" }); // Pale lavender sandals
            AddItem(new BodySash() { Hue = 1151, Name = "Note-Weaver's Sash" }); // Light purple sash

            AddItem(new Harp() { Hue = 1152, Name = "Harmonic Reed" }); // Decorative instrument

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Songkeeper's Pack";
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

