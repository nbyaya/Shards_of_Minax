using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VarlanDustwalkerQuest : BaseQuest
    {
        public override object Title { get { return "Echoes in the Dust"; } }

        public override object Description
        {
            get
            {
                return
                    "*Varlan’s eyes, shadowed by years of silence, meet yours with a quiet resolve.*\n\n" +
                    "\"I am Varlan Dustwalker, the last of the forsaken. My home, once a thriving place, now lies abandoned—cursed by the folly of its people, and my own cowardice. I must return, to face the past and silence the voices that haunt me. The road is perilous, and the spirits restless. Will you walk with me... through the dust of memory?\"";
            }
        }

        public override object Refuse { get { return "*Varlan’s voice fades like the wind.* \"Then may the dust keep its secrets, for now.\""; } }
        public override object Uncomplete { get { return "*Varlan tightens his grip on his staff.* \"The dead remember... and they stir. Let us not falter.\""; } }

        public VarlanDustwalkerQuest() : base()
        {
            AddObjective(new EscortObjective("a Ababdoned Town"));
            AddReward(new BaseReward(typeof(DarkKnightsDoomShield), "DarkKnightsDoomShield – A shield imbued with dark resistance and an aura that instills fear."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Varlan kneels, brushing the earth with his hand.* \"The past is at rest, and I owe that peace to you. Take this shield—it was once borne by my ancestors. May it guard you, as you have guarded me.\"", null, 0x59B);
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

    public class VarlanDustwalkerEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(VarlanDustwalkerQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public VarlanDustwalkerEscort() : base()
        {
            Name = "Varlan Dustwalker";
            Title = "the Last of the Forsaken";
            NameHue = 0x47E;
        }

		public VarlanDustwalkerEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1017; // Weathered, light tan skin
            HairItemID = 0x203B; // Short hair
            HairHue = 1102; // Ash brown
            FacialHairItemID = 0x2041; // Beard
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1157, Name = "Dustwalker's Shroud" }); // Desert-grey
            AddItem(new LeatherGloves() { Hue = 1164, Name = "Relickeeper's Grasp" }); // Faded bronze
            AddItem(new Sandals() { Hue = 2401, Name = "Silent Step Sandals" }); // Dusty bone
            AddItem(new Cloak() { Hue = 1175, Name = "Veil of the Abandoned" }); // Dull ash black
            AddItem(new SkullCap() { Hue = 1157, Name = "Hood of the Forsaken" }); // Matches robe

            AddItem(new WildStaff() { Hue = 1193, Name = "Echoroot Staff" }); // Ghostly teal, pulsating faintly

            Backpack backpack = new Backpack();
            backpack.Hue = 1164;
            backpack.Name = "Memory Satchel";
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
                        "*Varlan’s gaze drifts to the horizon.* I wonder... if they still wait for me, among the ruins.",
                        "*He tightens his cloak.* The wind carries their whispers... I cannot tell if they forgive me.",
                        "*He traces a symbol in the air.* This sigil... it once guarded our homes, now it is but a mark of failure.",
                        "*Varlan's voice lowers.* I fled when they stood, and I lived while they perished. Let me make it right.",
                        "*A chill lingers.* We are close. I can feel the dust remembering my name.",
                        "*He hums a low tune.* My mother sang this, to guide me in darkness. Now, I walk it alone.",
                        "*Varlan speaks to himself.* One step closer... to the end, or a new beginning?"
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
