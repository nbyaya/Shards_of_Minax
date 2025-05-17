using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

    public class ThargSkullsplitterQuest : BaseQuest
    {
        public override object Title { get { return "Escape from the Clan"; } }

        public override object Description
        {
            get
            {
                return 
                    "Tharg Skullsplitter eyes you with a mixture of defiance and desperation. 'I was once Bloodfist war-chief... now I’m just a hunted traitor. The clan plans war, and I know their every move. Take me to the Neutral Zone near Orc Territory, before my kin silence me for good. Help me live, and I’ll help you fight.'";
            }
        }

        public override object Refuse { get { return "Tharg snarls, 'Cowardice smells worse than blood... but I will find another way.'"; } }
        public override object Uncomplete { get { return "'We must move! My kin hunt with sharp eyes and sharper blades!'"; } }

        public ThargSkullsplitterQuest() : base()
        {
            AddObjective(new EscortObjective("Orc Territory"));
            AddReward(new BaseReward(typeof(WrestlersGrippingGloves), "WrestlersGrippingGloves – Boosts unarmed combat damage and strength"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Tharg grins fiercely. 'You have my thanks. Take these gloves, forged in battle, soaked in blood. Fight well, and remember—war is never far.'", null, 0x59B);
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

    public class ThargSkullsplitterEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(ThargSkullsplitterQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon()); // Tharg knows weapons, fitting his war-chief past
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ThargSkullsplitterEscort() : base()
        {
            Name = "Tharg Skullsplitter";
            Title = "the Defected Warchief";
            NameHue = 0x22; // Dark red hue for a fierce look
        }

		public ThargSkullsplitterEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 60);
            Female = false;
            CantWalk = false;
            Hue = 0x8401;
            HairItemID = 0x2044;
            HairHue = 1107;
            FacialHairItemID = 0x203E;
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new OrcMask() { Hue = 0x455 }); // Orcish heritage
            AddItem(new StuddedChest() { Hue = 0x96D });
            AddItem(new StuddedLegs() { Hue = 0x96D });
            AddItem(new LeatherGloves() { Hue = 0x966 });
            AddItem(new OrcHelm() { Hue = 0x455 });
            AddItem(new WarAxe()); // Former war-chief, favors brutal weapons
            AddItem(new BodySash() { Hue = 0x455 });
            AddItem(new Boots() { Hue = 0x1BB });
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15) // 15% chance to talk each tick
                {
                    string[] lines = new string[]
                    {
                        "'Keep moving. The Bloodfist never stop hunting.'",
                        "*Tharg glances over his shoulder* 'I can feel their eyes... sharpened by hate.'",
                        "'I used to lead them. Now they want my head as a trophy.'",
                        "*Tharg growls* 'I won’t die in the dirt. Not today.'",
                        "'The Neutral Zone is close... I can almost taste freedom.'",
                        "'You think orcs are savages? Some are worse—clever, cruel, relentless.'",
                        "*He grips his axe tightly* 'They taught me to kill. Now, I use it to survive.'"
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
