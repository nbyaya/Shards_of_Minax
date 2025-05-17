using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SeraphineDreamQuest : BaseQuest
    {
        public override object Title { get { return "The Final Canvas"; } }

        public override object Description
        {
            get
            {
                return 
                    "You found me... I am Seraphine Dreamtide. For years, I have painted visions I cannot explain—visions not my own. A muse follows me, whispers to me, drives me to madness. I must return to Fawn, to trap this muse in one final work... before it consumes me. Will you escort me there? Please, it watches even now...";
            }
        }

        public override object Refuse { get { return "The muse never sleeps. It will not let me rest. I must return... even if I must walk alone."; } }
        public override object Uncomplete { get { return "Hurry... I feel it pulling at my mind again, like threads unraveling."; } }

        public SeraphineDreamQuest() : base()
        {
            AddObjective(new EscortObjective("the town of Fawn"));
            AddReward(new BaseReward(typeof(RoguesShadowCloak), "Rogue's Shadow Cloak"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you... With your help, I can finally paint in peace. The muse... is silent, for now. Take this cloak—may it help you slip through shadows as I once slipped through dreams.", null, 0x59B);
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

    public class SeraphineDreamEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(SeraphineDreamQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public SeraphineDreamEscort() : base()
        {
            Name = "Seraphine Dreamtide";
            Title = "the Haunted Painter";
            NameHue = 0x83E;
        }

		public SeraphineDreamEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(40, 60, 25);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83F8;
            HairItemID = 0x203B;
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 0x48C });
            AddItem(new Cloak() { Hue = 0x455 });
            AddItem(new Sandals() { Hue = 0x4B0 });
            AddItem(new FeatheredHat() { Hue = 0x48C });
            AddItem(new PaintsAndBrush()); // Custom decorative item
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
                        "*Seraphine gazes into the distance* 'Do you see it too? The muse... it hides behind the light.'",
                        "'Each step we take brings me closer to the final stroke... to silence the whispers.'",
                        "'The colors... they bleed when I close my eyes. Please, let's keep moving.'",
                        "*She shudders* 'It’s watching us. I can feel it in the brushstrokes of the wind.'",
                        "'Fawn... the only place where the muse dared show its true face. We must hurry.'",
                        "'Do you ever wonder if your dreams are your own? Or do they belong to something else?'"
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
