using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HarakTheSandwalkerQuest : BaseQuest
    {
        public override object Title { get { return "Pharaoh's Curse"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Harak’s golden eyes glint beneath his desert scarf, his voice low and hurried.*\n\n" +
                    "\"You... you must aid me. I am Harak, a wanderer cursed by mistaken fate. The dead rise, believing me a pharaoh returned. I must reach the Ancient Pyramid to uncover the truth of this curse and end their pursuit. Will you guide me through the sands, while I still draw breath?\"";
            }
        }

        public override object Refuse { get { return "*Harak bows his head, shadows dancing across his face.* \"Then may the sands swallow me, and the dead find what they seek.\""; } }
        public override object Uncomplete { get { return "*Harak looks back, haunted.* \"The dead draw near... we must not falter.\""; } }

        public HarakTheSandwalkerQuest() : base()
        {
            AddObjective(new EscortObjective("the Ancient Pyramid"));
            AddReward(new BaseReward(typeof(EssenceOfToad), "EssenceOfToad – Grants resistance to poison and a mighty leap for a time."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Harak places a small vial in your hand.* \"You have saved me, friend of the sands. Take this, a gift brewed from desert lore. Leap as the toad, and resist what poisons seek your end.\"", null, 0x59B);
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

    public class HarakTheSandwalkerEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(HarakTheSandwalkerQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public HarakTheSandwalkerEscort() : base()
        {
            Name = "Harak";
            Title = "the Sandwalker";
            NameHue = 0x57D; // Sandy golden hue
        }

		public HarakTheSandwalkerEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 65, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2411; // Desert-tanned skin
            HairItemID = 0x203B; // Short hair
            HairHue = 1147; // Dark sandy brown
            FacialHairItemID = 0x2041; // Trimmed beard
            FacialHairHue = 1147;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 2213, Name = "Sandcloak of the Exile" }); // Dusty golden-brown cloak
            AddItem(new LeatherDo() { Hue = 2220, Name = "Nomad's Leather Vestments" }); // Worn desert leather
            AddItem(new LeatherHaidate() { Hue = 2101, Name = "Dune-Walker Leggings" }); // Pale sand leggings
            AddItem(new NinjaTabi() { Hue = 2218, Name = "Silent Step Sandals" }); // Light desert sandals
            AddItem(new BodySash() { Hue = 1161, Name = "Sash of Scorched Suns" }); // Deep sunset red
            AddItem(new TribalMask() { Hue = 1109, Name = "Veil of Forgotten Kings" }); // Faded bone-white mask (worn around neck)

            AddItem(new GnarledStaff() { Hue = 2412, Name = "Desert Wind Staff" }); // Pale wood staff carved with glyphs

            Backpack backpack = new Backpack();
            backpack.Hue = 2213;
            backpack.Name = "Nomad's Reliquary";
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
                        "*Harak glances over his shoulder.* The sand stirs... they follow.",
                        "*His voice is hushed.* I am not who they think I am... but the dead care little for truth.",
                        "*Harak grips his staff tightly.* The pyramid... it holds the answer, or my doom.",
                        "*His eyes narrow.* Each step in the sand echoes with old blood.",
                        "*Harak breathes heavily.* The pharaoh's curse is not mine to bear... yet it hunts me still.",
                        "*He looks to the horizon.* When I reach the pyramid, I shall know if I live... or if I awaken the past.",
                        "*Harak murmurs a prayer.* Spirits of the dune, shield us from what crawls beneath."
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
