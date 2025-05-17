using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TobanDustwalkerQuest : BaseQuest
    {
        public override object Title { get { return "The Star That Broke"; } }

        public override object Description
        {
            get
            {
                return
                    "*Toban adjusts the tattered scarf covering his face, revealing eyes that shimmer with starlight.*\n\n" +
                    "They call me Toban Dustwalker. I've traveled the length of Sosaria, fleeing shadows cast by a star that fell, and broke. This shard I carry... it calls to them. If I don’t reach the Shingorr Inn before the next moonrise, they will find me—and the shard will return to darkness. Will you guide me? I’ll reward you with armor meant to hold the line, even when the heavens fall.";
            }
        }

        public override object Refuse { get { return "*Toban lowers his gaze, the shard pulsing faintly.* Then let the dust claim me as it did them."; } }
        public override object Uncomplete { get { return "*Toban grips his side, wincing.* They draw near... we must keep moving, or all is lost."; } }

        public TobanDustwalkerQuest() : base()
        {
            AddObjective(new EscortObjective("Shingorr Inn"));
            AddReward(new BaseReward(typeof(ShaftstopArmor), "ShaftstopArmor – Heavy armor that resists knockback and piercing."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Toban hands you a heavy piece of armor, etched with cosmic sigils.* You’ve given me a chance... now take this, and stand firm when the stars shift. We may yet meet again, under clearer skies.", null, 0x59B);
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

    public class TobanDustwalkerEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(TobanDustwalkerQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVagabond());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public TobanDustwalkerEscort() : base()
        {
            Name = "Toban Dustwalker";
            Title = "the Star-Broken";
            NameHue = 0x480; // Slightly dark hue
        }

		public TobanDustwalkerEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1024; // Weathered skin tone
            HairItemID = 0x2049; // Messy short hair
            HairHue = 1102; // Dusky brown
            FacialHairItemID = 0x204B; // Rugged beard
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Dustwalker’s Shroud" }); // Dark sand-brown hooded cloak
            AddItem(new LeatherGloves() { Hue = 1109, Name = "Stargazer’s Grasp" }); // Dust-grey leather gloves
            AddItem(new LeatherLegs() { Hue = 1108, Name = "Wanderer’s Treads" }); // Faded grey-brown legs
            AddItem(new Boots() { Hue = 1107, Name = "Scorched Path Boots" }); // Weathered black boots
            AddItem(new BodySash() { Hue = 1150, Name = "Shard-Bound Sash" }); // Midnight black sash with faintly glowing runes

            AddItem(new Scepter() { Hue = 1153, Name = "Starshard Rod" }); // A metallic rod that flickers with dim blue light

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Dustwalker’s Pack";
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
                        "*Toban clutches the shard beneath his cloak.* They are near... I can feel their pull.",
                        "*He stares at the sky.* The stars... they don’t shine the same since that night.",
                        "*Toban murmurs.* I’ve crossed deserts and shadows, but the shard still sings.",
                        "*He grips his rod tightly.* This weapon... a fragment of what once was. It won’t hold them forever.",
                        "*A low hum fills the air as Toban’s sash flickers.* We’re running out of time. We must hurry.",
                        "*Toban glances back.* Do you see them? No? Good... but they see us.",
                        "*He breathes deeply.* The Shingorr Inn... one last shelter before I vanish again."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
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
