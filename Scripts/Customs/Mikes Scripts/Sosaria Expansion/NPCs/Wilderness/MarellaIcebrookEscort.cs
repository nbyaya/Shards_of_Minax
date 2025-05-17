using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MarellaIcebrookQuest : BaseQuest
    {
        public override object Title { get { return "The Frostbound Pact"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Marella’s eyes shimmer like frozen lakes, her voice soft but urgent.*\n\n" +
                    "The frost spirits stir restlessly, their pact fraying with each passing moment. I must return to Vinterdale Isle to renew our ancient bond before the cold becomes wrathful. Alone, I may not make it—will you guide me across these lands before the winter breaks loose?";
            }
        }

        public override object Refuse { get { return "*A thin mist gathers around her.* Then may the frost find me swiftly, and spare the rest."; } }
        public override object Uncomplete { get { return "*Her breath clouds the air.* The spirits grow impatient—we must hurry."; } }

        public MarellaIcebrookQuest() : base()
        {
            AddObjective(new EscortObjective("Vinterdale Isle"));
            AddReward(new BaseReward(typeof(SilkRoadTreasuresChest), "SilkRoadTreasuresChest – Exotic treasures from distant lands."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Marella clasps your hand, a chill lingering.* The frost sleeps once more. You have my gratitude, and this token of rare beauty from distant climes. May it remind you of our fragile peace.", null, 0x480);
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

    public class MarellaIcebrookEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(MarellaIcebrookQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public MarellaIcebrookEscort() : base()
        {
            Name = "Marella Icebrook";
            Title = "the Frostbound Mage";
            NameHue = 0x480; // Icy blue
        }

		public MarellaIcebrookEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, frost-kissed skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1152; // Silvery white
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleElvenRobe() { Hue = 1153, Name = "Robe of Eternal Frost" }); // Frost-blue silk
            AddItem(new Cloak() { Hue = 1154, Name = "Mistcloak of Vinterdale" }); // Pale icy mist
            AddItem(new LeatherGloves() { Hue = 1150, Name = "Snowbound Grasp" }); // White leather
            AddItem(new Sandals() { Hue = 1170, Name = "Steps of Silence" }); // Crystal-grey
            AddItem(new WizardsHat() { Hue = 1153, Name = "Helm of Winter’s Gaze" }); // Matches robe

            AddItem(new WildStaff() { Hue = 1152, Name = "Shardwood Staff" }); // Ice-wood infused staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Satchel of Frost Scrolls";
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
                        "*Marella's breath mists in the air.* The spirits whisper warnings... we must not delay.",
                        "*She looks to the sky.* The frost watches us. It is not yet angered, but it is close.",
                        "*Her hand traces a glowing rune in the air.* This path grows colder, even for me...",
                        "*She tightens her cloak.* Vinterdale must be near. I can feel the spirits stir.",
                        "*Marella hums a soft, chilling tune.* An old song, to soothe the frost before it cuts.",
                        "*She glances back.* Do you feel that? The air grows sharp. The pact must be renewed soon.",
                        "*Her voice carries an edge of fear.* If we fail, winter shall not wait for spring..."
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
