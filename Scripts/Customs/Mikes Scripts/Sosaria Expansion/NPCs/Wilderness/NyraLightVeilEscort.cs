using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class NyraLightVeilQuest : BaseQuest
    {
        public override object Title { get { return "Through Veils of Light"; } }

        public override object Description
        {
            get
            {
                return
                    "*Nyra’s eyes, veiled in shimmering silk, seem to look beyond you.*\n\n" +
                    "\"I am Nyra of the Light Veil, once a seer of Dawn, now adrift in shadow. The etherial version of Dawn awaits me, where light and vision are one. I must return, for a prophecy stirs, and my sight can only be reclaimed there. Will you walk with me through unseen paths, and help me find my way once more?\"";
            }
        }

        public override object Refuse { get { return "*Nyra’s voice is soft, almost fading.* \"Then the light dims... but I shall find another way.\""; } }
        public override object Uncomplete { get { return "*Her hand trembles as she clutches her staff.* \"The path is unclear... but we must press on.\""; } }

        public NyraLightVeilQuest() : base()
        {
            AddObjective(new EscortObjective("the etherial town of Dawn"));
            AddReward(new BaseReward(typeof(SabatonsOfDawn), "SabatonsOfDawn – Boots that allow brief levitation and resistance to magic traps."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Nyra smiles faintly as light seems to coalesce around her.* \"The veil lifts, if only for a moment. Take these, traveler—walk as I now do, above the earth and beyond its snares. May the light guide your path.\"", null, 0x47E);
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

    public class NyraLightVeilEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(NyraLightVeilQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public NyraLightVeilEscort() : base()
        {
            Name = "Nyra";
            Title = "of the Light Veil";
            NameHue = 1153;
        }

		public NyraLightVeilEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 65, 75);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, luminous skin tone
            HairItemID = 0x2049; // Flowing long hair
            HairHue = 1153; // Light silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1153, Name = "Veil of Ethereal Sight" }); // Light silver-blue veil
            AddItem(new PlainDress() { Hue = 1154, Name = "Robe of the Inner Light" }); // Soft, shimmering white-blue dress
            AddItem(new Cloak() { Hue = 1157, Name = "Shroud of Silent Paths" }); // Soft lilac cloak
            AddItem(new Sandals() { Hue = 1151, Name = "Whisperstep Sandals" }); // Pale glowing sandals
            AddItem(new BodySash() { Hue = 1152, Name = "Lightbinder's Sash" }); // Soft gold sash

            AddItem(new WildStaff() { Hue = 1150, Name = "Staff of the Fading Star" }); // Pale staff with light particles

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Visionkeeper's Pack";
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
                        "*Nyra tilts her head.* The light bends strangely here... we must be cautious.",
                        "*She hums softly.* The melody helps me see the path, even in darkness.",
                        "*Nyra whispers.* Do you feel it? The veil is thin... the Etherial Dawn draws near.",
                        "*Her eyes flutter beneath the veil.* I dream of colors I can no longer name.",
                        "*She grips her staff.* The prophecy awaits... I must not falter.",
                        "*Nyra breathes deeply.* Light is not seen... it is felt, known, trusted.",
                        "*She murmurs.* Each step brings me closer to truth, or to nothingness."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
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
