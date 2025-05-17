using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BrynnaHollowshadeQuest : BaseQuest
    {
        public override object Title { get { return "Echoes in the Glow"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Brynna’s hands tremble slightly, her eyes reflecting a strange bioluminescent hue.*\n\n" +
                    "\"The caverns… I went for the fungi. They glow, you see, they whisper, and I thought I could harness them. But now... I hear them still. I fear I’ve brought something back. I must return, must finish what I began... Will you take me there? Keep me safe? Before they claim my mind completely.\"";
            }
        }

        public override object Refuse { get { return "*Brynna shrinks back, clutching her satchel.* \"Then I must go alone... even if I lose myself.\""; } }
        public override object Uncomplete { get { return "*She grips your arm, her voice frantic.* \"No! We cannot stop now... The glow is calling!\""; } }

        public BrynnaHollowshadeQuest() : base()
        {
            AddObjective(new EscortObjective("the Caverns under Dawn"));
            AddReward(new BaseReward(typeof(HerbalistsProtectiveHat), "HerbalistsProtectiveHat – Grants protection from poisonous spores and increases herbalism skill."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Brynna hands you a finely woven hat, glowing faintly.* \"Thank you... They’re quieter now. Perhaps I can rest. May this aid you, as you aided me.\"", null, 0x59B);
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

    public class BrynnaHollowshadeEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(BrynnaHollowshadeQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public BrynnaHollowshadeEscort() : base()
        {
            Name = "Brynna Hollowshade";
            Title = "the Glow-Seeker";
            NameHue = 0x48D; // Slightly luminous
        }

		public BrynnaHollowshadeEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale, moonlit skin tone
            HairItemID = 0x203B; // Wavy hair
            HairHue = 1153; // Light lavender
        }

        public override void InitOutfit()
        {
            AddItem(new ElvenShirt() { Hue = 1260, Name = "Glowthread Tunic" }); // Iridescent blue-green
            AddItem(new LeatherSkirt() { Hue = 2117, Name = "Sporeshade Skirt" }); // Deep forest green
            AddItem(new Cloak() { Hue = 1275, Name = "Cloak of Fungal Dreams" }); // Faintly glowing purple
            AddItem(new Sandals() { Hue = 1193, Name = "Mosswoven Sandals" }); // Earthy brown-green
            AddItem(new BodySash() { Hue = 1166, Name = "Lichen Sash" }); // Pale green
            AddItem(new WideBrimHat() { Hue = 1270, Name = "Hollowshade Hat" }); // Violet with glowing trim

            AddItem(new Bag() { Hue = 1109, Name = "Satchel of Spores" }); // Contains strange spores

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Fungal Pouch";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    string[] lines = new string[]
                    {
                        "*Brynna touches her temple.* Do you hear them? The whispers? They never stop...",
                        "*Her eyes dart around.* The light… it’s in my dreams now. Bright and hungry.",
                        "*She clutches her satchel.* If I lose my way, remind me who I am… please.",
                        "*A shiver runs through her.* The caverns are alive… and they remember me.",
                        "*She murmurs softly.* I thought knowledge was worth the risk. I was wrong.",
                        "*Her breath quickens.* We must not tarry. The glow grows stronger by the hour."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }

}
