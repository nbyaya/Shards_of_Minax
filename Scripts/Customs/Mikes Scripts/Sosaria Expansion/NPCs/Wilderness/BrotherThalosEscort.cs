using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BrotherThalosQuest : BaseQuest
    {
        public override object Title { get { return "The Long Walk of Redemption"; } }

        public override object Description
        {
            get
            {
                return
                    "*Thalos stands tall, his voice calm but his eyes stormy with memory.*\n\n" +
                    "\"I am Brother Thalos, once called the Crimson Warlord, now but a man seeking peace. Visions plague me—ghosts of those I wronged. I must reach the Cathedral to offer my penance, or these visions will claim me. Will you walk beside me, through shadow and judgment, to the light I seek?\"";
            }
        }

        public override object Refuse { get { return "*Thalos bows solemnly.* \"Then may the burden remain mine to bear alone.\""; } }
        public override object Uncomplete { get { return "*Thalos breathes heavily.* \"The past hunts me still... we must not tarry.\""; } }

        public BrotherThalosQuest() : base()
        {
            AddObjective(new EscortObjective("a Cathedral"));
            AddReward(new BaseReward(typeof(WhisperersBoots), "WhisperersBoots – Light boots that silence footsteps and increase stealth."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Thalos kneels at the Cathedral steps.* \"You have my eternal thanks. May these boots serve you as silently as I now walk the path of redemption.\"", null, 0x59B);
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

    public class BrotherThalosEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(BrotherThalosQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMonk());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public BrotherThalosEscort() : base()
        {
            Name = "Brother Thalos";
            Title = "the Redeemed";
            NameHue = 0x47E;
        }

		public BrotherThalosEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1052; // Weathered skin tone
            HairItemID = 0x2049; // Topknot
            HairHue = 1109; // Silver
            FacialHairItemID = 0x204D; // Long Beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1157, Name = "Shroud of Atonement" }); // Deep crimson, symbol of penance
            AddItem(new Sandals() { Hue = 1175, Name = "Steps of Silence" }); // Pale grey, quiet footfalls
            AddItem(new BodySash() { Hue = 1150, Name = "Sash of the Redeemed" }); // Black with crimson lining
            AddItem(new Cloak() { Hue = 1153, Name = "Burden's Cloak" }); // Midnight blue, weight of the past
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Hands Once Stained" }); // Ash grey gloves, scarred

            AddItem(new GnarledStaff() { Hue = 2115, Name = "Staff of the Fallen Banner" }); // Dark wood, worn by time

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Penance Pack";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.12)
                {
                    string[] lines = new string[]
                    {
                        "*Thalos grips his staff tightly.* Every step forward is a step away from who I was.",
                        "*His eyes close briefly.* They watch from the shadows... the fallen never forget.",
                        "*You hear him whisper.* Will the Cathedral forgive what the battlefield cannot?",
                        "*Thalos gazes at the horizon.* The road is long, but not as long as the trail of my sins.",
                        "*A cold wind stirs.* The past is a weight, but it shall not break me again.",
                        "*Thalos hums an old war hymn, now turned into a prayer.* From blood I came, to peace I walk.",
                        "*He touches the sash at his waist.* This was once soaked in crimson... now it binds me to truth."
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
