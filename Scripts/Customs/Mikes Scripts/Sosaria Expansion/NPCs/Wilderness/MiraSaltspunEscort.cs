using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MiraSaltspunQuest : BaseQuest
    {
        public override object Title { get { return "Threads of the Tides"; } }

        public override object Description
        {
            get
            {
                return
                    "The ocean’s breath has guided you to me... My name is Mira Saltspun, weaver of seafoam and silk. But I am not merely running from the past—no, I am chased by threads cursed by a jealous rival. " +
                    "These threads... they twist and writhe, seeking to bind me to an unfinished promise. A spectral customer awaits in Grey, one whose cloak I must finish, lest the curse claim me whole. " +
                    "Will you walk with me back to Grey? I dare not take this journey alone, for each step grows heavier with the weight of woven fates.";
            }
        }

        public override object Refuse { get { return "Then the tides may yet claim me, and the loom will fall silent..."; } }
        public override object Uncomplete { get { return "The threads pull tighter... please, let us not delay our journey to Grey."; } }

        public MiraSaltspunQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Grey"));
            AddReward(new BaseReward(typeof(CourtierSilkenRobe), "Courtier Silken Robe"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("The cloak is complete, and so too is my debt. The curse lifts with the sea breeze... Take this robe, may charm and grace follow you as the tides do the moon.", null, 0x59B);
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

    public class MiraSaltspunEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(MiraSaltspunQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaver());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public MiraSaltspunEscort() : base()
        {
            Name = "Mira Saltspun";
            Title = "the Seafoam Weaver";
            NameHue = 0x83F;
        }

		public MiraSaltspunEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(40, 50, 30);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x2049; // Long Hair
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 0x481 }); // Sea-green dress
            AddItem(new Cloak() { Hue = 0x47E }); // Soft blue cloak
            AddItem(new Sandals() { Hue = 0x59C }); // Soft beige sandals
            AddItem(new BodySash() { Hue = 0x47F }); // Ocean-blue sash
            AddItem(new SewingNeedle());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.1) // 10% chance to talk each tick
                {
                    string[] lines = new string[]
                    {
                        "*Mira gently touches her sash* 'The seafoam keeps me safe... or so I hope.'",
                        "'Do you hear it? The waves whisper of unfinished promises.'",
                        "*She hums a tune, soft and lilting, like the tides.*",
                        "'Grey is close now... I can feel the pull of the loom.'",
                        "*She shudders* 'The threads twist on their own... they want to bind, not create.'",
                        "'I once believed the sea would wash all curses away. Now I know better.'",
                        "*Mira glances over* 'You are kind, to walk with me. Most would turn at the mention of the spectral.'"
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
