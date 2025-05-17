using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ChronoReckoningQuest : BaseQuest
    {
        public override object Title { get { return "The Hour Repeats"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Zell’s eyes flicker, as if seeing multiple timelines at once.*\n\n" +
                    "\"I am Chronomancer Zell. I’ve made a terrible error—the hour repeats, again and again, and I cannot escape it alone. " +
                    "I must reach Malidor Witches Academy before the 60th chime or remain trapped forever. I remember you... or will... perhaps. " +
                    "Will you help me break the loop and restore time's flow?\"";
            }
        }

        public override object Refuse { get { return "*Zell's form shimmers briefly.* \"Then I shall ask again... or have I already?\""; } }
        public override object Uncomplete { get { return "*The sound of ticking grows louder.* \"We must hurry. The hourglass empties... again.\""; } }

        public ChronoReckoningQuest() : base()
        {
            AddObjective(new EscortObjective("the Time Awaits Dungeon"));
            AddReward(new BaseReward(typeof(BabyLavos), "BabyLavos – A rare pet with minor time-altering abilities."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Zell bows, relieved.* \"Time... it flows once more. This companion is a shard of the cycle, may it bend moments in your favor. Farewell, friend—until the next loop... or perhaps never again.\"", null, 0x488);
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

    public class ChronomancerZellEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(ChronoReckoningQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ChronomancerZellEscort() : base()
        {
            Name = "Chronomancer Zell";
            Title = "the Loopbound";
            NameHue = 1153;
        }

		public ChronomancerZellEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 60, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Slightly ethereal skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1157; // Deep indigo
            FacialHairItemID = 0x204B; // Long beard
            FacialHairHue = 1157;
        }

        public override void InitOutfit()
        {
            AddItem(new WizardsHat() { Hue = 1161, Name = "Hat of Twilit Hours" }); // Dark violet
            AddItem(new Robe() { Hue = 1175, Name = "Chronoweave Robe" }); // Midnight blue with silver trim
            AddItem(new Sandals() { Hue = 1109, Name = "Temporal Striders" }); // Dust-grey
            AddItem(new BodySash() { Hue = 1154, Name = "Sash of Endless Time" }); // Light azure
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Hands of the Loopbound" }); // Dark silver
            AddItem(new Cloak() { Hue = 1178, Name = "Cloak of the Forgotten Second" }); // Deep dusk

            AddItem(new WildStaff() { Hue = 1164, Name = "Chronoscepter" }); // Twilight hue staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Temporal Satchel";
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
                        "*Zell’s eyes dart.* Have we walked this road already? Or is it still to come?",
                        "*You feel a strange déjà vu.* Time unravels... We must be swift.",
                        "*Zell murmurs.* Tick... tick... tick... The hour repeats, as do I.",
                        "*He looks at you with sudden clarity.* I remember this moment, but differently. We must change the outcome.",
                        "*Zell winces.* The past echoes loudly here. I can hear it calling.",
                        "*The air distorts slightly.* Each step forward may be a step back if we delay.",
                        "*Zell hums an eerie tune.* The song of the hourglass... have you heard it too?",
                        "*His voice trembles.* I sealed the spell poorly... I need the Academy's help to break it.",
                        "*Zell clutches his staff.* If I falter, the loop will begin anew—do not let me fall."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
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
