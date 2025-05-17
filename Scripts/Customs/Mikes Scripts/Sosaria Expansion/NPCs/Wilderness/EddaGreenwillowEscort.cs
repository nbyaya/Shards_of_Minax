using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EddaGreenwillowQuest : BaseQuest
    {
        public override object Title { get { return "Roots of Renewal"; } }

        public override object Description
        {
            get
            {
                return
                    "Greetings, kind soul. I am Edda Greenwillow, a druidess of the old groves. I must reach the sacred woods of Yew before the next solstice. This staff I bear holds the last seed of the Elder Tree, and I must plant it where the leyline breathes strongest. The roads are treacherous, and shadows stir where once was peace. Will you guide me to safety?";
            }
        }

        public override object Refuse { get { return "The seed cannot wait, nor can I delay its planting. Should you change your heart, seek me quickly..."; } }
        public override object Uncomplete { get { return "We must make haste, the solstice draws near and the seed grows restless in its slumber."; } }

        public EddaGreenwillowQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Yew"));
            AddReward(new BaseReward(typeof(BowcraftersProtectiveCloak), "Bowcrafter's Protective Cloak"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you, guardian. The seed now rests where it belongs. May your aim be true, and your spirit grounded like the deepest roots.", null, 0x59B);
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

    public class EddaGreenwillowEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(EddaGreenwillowQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public EddaGreenwillowEscort() : base()
        {
            Name = "Edda Greenwillow";
            Title = "the Wandering Druidess";
            NameHue = 0x59B;
        }

		public EddaGreenwillowEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 60, 50);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83F4;
            HairItemID = 0x2049;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 0x59B });
            AddItem(new Cloak() { Hue = 0x594 });
            AddItem(new Sandals() { Hue = 0x515 });
            AddItem(new FlowerGarland() { Hue = 0x59C });
            AddItem(new GnarledStaff());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    string[] lines = new string[]
                    {
                        "*Edda caresses the staff, murmuring* 'Steady now, little seed... soon, you will wake.'",
                        "'The trees whisper warnings, but I do not fear. Nature protects those who walk gently.'",
                        "*She gazes ahead* 'These lands have changed... even the stones feel different underfoot.'",
                        "'Do you smell that? The wind carries the scent of old magic, waiting to return.'",
                        "*Edda hums softly* 'A song of the groves... may it guide us safely.'",
                        "'Thank you for walking with me. The seed senses your kindness.'",
                        "*She pauses briefly* 'Not much farther now... the roots will soon remember their place.'"
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
