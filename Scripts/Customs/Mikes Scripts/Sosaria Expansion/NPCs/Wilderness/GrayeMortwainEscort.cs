using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GrayeMortwainQuest : BaseQuest
    {
        public override object Title { get { return "To Rest, At Last"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Graye’s pale eyes meet yours, weary but determined.*\n\n" +
                    "I am Graye Mortwain, keeper of graves, steward of the forgotten. My time among the living wanes, and I must return to the Cemetery to perform my final rites. But a spirit clings to me, one I failed to lay to rest. The farther I roam, the angrier it becomes. Will you guide me, before it consumes us both?";
            }
        }

        public override object Refuse { get { return "*Graye sighs, his voice distant.* Then may the earth forgive me for lingering."; } }
        public override object Uncomplete { get { return "*The air grows colder around Graye.* The spirit stirs... we must continue."; } }

        public GrayeMortwainQuest() : base()
        {
            AddObjective(new EscortObjective("a Cemetary"));
            AddReward(new BaseReward(typeof(LuckyDice), "LuckyDice – Enhances fortune in loot and minor chance events."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Graye’s form shimmers as he bows.* Thank you, friend. The earth calls, and I shall answer. Take this, a token of passing fortunes. May luck favor you where shadows tread.", null, 0x59B);
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

    public class GrayeMortwainEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(GrayeMortwainQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public GrayeMortwainEscort() : base()
        {
            Name = "Graye Mortwain";
            Title = "the Gravekeeper";
            NameHue = 0x482;
        }

		public GrayeMortwainEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 50, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Pale skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1108; // Ash grey
            FacialHairItemID = 0x2041; // Beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1150, Name = "Mortwain's Shroud" }); // Deep, faded black
            AddItem(new LeatherGloves() { Hue = 1109, Name = "Embalmer's Touch" }); // Dust-grey
            AddItem(new Boots() { Hue = 1175, Name = "Grave-Treader Boots" }); // Shadowed brown
            AddItem(new Cloak() { Hue = 1153, Name = "Cloak of the Forgotten" }); // Midnight blue
            AddItem(new SkullCap() { Hue = 1150, Name = "Mortwain's Cap" }); // Matches robe

            AddItem(new GnarledStaff() { Hue = 1102, Name = "Gravestake" }); // Bone-colored staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Reliquary Pack";
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
                        "*Graye shivers.* It grows closer... the spirit is never far.",
                        "*You feel a cold breeze.* It’s angry... it knows I flee my duty.",
                        "*Graye clutches his staff.* Do not look behind us... sometimes, it takes shape.",
                        "*His voice trembles.* I failed to bury a truth, and now it haunts me.",
                        "*A shadow passes over him.* We must reach the Cemetery, or all is lost.",
                        "*Graye hums a solemn tune, lost in thought.* This was once a lullaby for the dead.",
                        "*Graye speaks softly.* When I am gone, will you tend the graves?"
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