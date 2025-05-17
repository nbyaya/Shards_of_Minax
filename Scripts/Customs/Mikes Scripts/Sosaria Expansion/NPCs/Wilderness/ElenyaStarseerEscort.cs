using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElenyaStarseerQuest : BaseQuest
    {
        public override object Title { get { return "Under Starlit Paths"; } }

        public override object Description
        {
            get
            {
                return
                    "You must help me! I am Elenya Starseer, and the stars... they whisper urgent truths! " +
                    "The next lunar alignment draws near, and I must return to Moon to witness it—if I miss this moment, the cosmos may never speak to me again. " +
                    "Please, guide me safely back before the celestial gates close.";
            }
        }

        public override object Refuse { get { return "The stars weep for those who ignore their call... I only hope they will forgive you."; } }
        public override object Uncomplete { get { return "Time slips like sand, traveler. Each step delays destiny itself."; } }

        public ElenyaStarseerQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Moon"));
            AddReward(new BaseReward(typeof(MeteoriteShard), "Meteorite Shard - a rare crafting gem of celestial power"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you, dear traveler. The stars are singing once more... take this meteorite shard, a gift fallen from the heavens.", null, 0x59B);
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

    public class ElenyaStarseerEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElenyaStarseerQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ElenyaStarseerEscort() : base()
        {
            Name = "Elenya Starseer";
            Title = "the Dreaming Astrologer";
            NameHue = 0x5BE;
        }

		public ElenyaStarseerEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 60, 30);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x203B;
            HairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 0x488 });
            AddItem(new Cloak() { Hue = 0x47F });
            AddItem(new Sandals() { Hue = 0x481 });
            AddItem(new WizardsHat() { Hue = 0x489 });
            AddItem(new MagicWand());
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
                        "*Elenya gazes skyward* 'The stars shift... can you feel their song?'",
                        "'They told me you would come. The constellations always speak in riddles.'",
                        "'Each step brings us closer... the moon's light grows stronger.'",
                        "*She hums softly* 'By starlight we travel, by starlight we are watched.'",
                        "'Stray not from this path... the Void stirs when the stars sleep.'",
                        "'Soon we will see the Celestial Spire... soon I will hear them clearly again.'",
                        "*Elenya's eyes sparkle* 'I have waited lifetimes for this night.'"
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

    // Reward Item: MeteoriteShard
    public class MeteoriteShard : Item
    {
        [Constructable]
        public MeteoriteShard() : base(0x1F19)
        {
            Name = "Meteorite Shard";
            Hue = 0x47E;
            Weight = 1.0;
        }

        public MeteoriteShard(Serial serial) : base(serial) { }

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
