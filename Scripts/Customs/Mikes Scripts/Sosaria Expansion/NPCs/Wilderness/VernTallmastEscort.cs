using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VernTallmastQuest : BaseQuest
    {
        public override object Title { get { return "Light in the Fog"; } }

        public override object Description
        {
            get
            {
                return
                    "You’ve found me, thank the stars! I’m Vern Tallmast, once keeper of the Lighthouse… until they cast me out for what I saw. " +
                    "A ship of the dead, they said I was mad, delirious from the sea fog! But now, a storm brews and I know the crew of the *Wailing Star* is out there, " +
                    "trapped between worlds, needing my light to guide them home. Please, take me back to the Lighthouse—I must light the beacon before it’s too late.";
            }
        }

        public override object Refuse { get { return "I cannot stay in this shadow any longer… but if you leave me, so be it. The sea will claim them, and me too."; } }
        public override object Uncomplete { get { return "The waves are growing louder… please, hurry. The *Wailing Star* won’t last the night."; } }

        public VernTallmastQuest() : base()
        {
            AddObjective(new EscortObjective("the Lighthouse"));
            AddReward(new BaseReward(typeof(ArcadeMastersVault), "ArcadeMastersVault – A mysterious container with rare trinkets and oddities"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("You’ve done more than save lives—you’ve restored my light. May these oddities guide you in times of darkness.", null, 0x59B);
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

    public class VernTallmastEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(VernTallmastQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFisherman()); // As a former lighthouse keeper, fisherman fits his sea-faring roots
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public VernTallmastEscort() : base()
        {
            Name = "Vern Tallmast";
            Title = "the Exiled Lighthouse Keeper";
            NameHue = 0x83F;
        }

		public VernTallmastEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 50, 30);
            Female = false;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x8401;
            HairItemID = 0x2049;
            HairHue = 1150;
            FacialHairItemID = 0x203C;
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 0x497 });
            AddItem(new Cloak() { Hue = 0x455 });
            AddItem(new TallStrawHat() { Hue = 0x59B });
            AddItem(new Boots() { Hue = 0x481 });
            AddItem(new CampingLanturn()); // His old lighthouse lantern
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.12) // 12% chance to talk each tick
                {
                    string[] lines = new string[]
                    {
                        "*Vern scans the horizon* 'Do you see them too? The shadows beneath the waves…'",
                        "'They called me mad, but I saw it—a ship of souls, lost and crying for light.'",
                        "*He tightens his grip on the lantern* 'This light has saved hundreds… and it will save them too.'",
                        "'The fog… it’s thicker than I remember. Keep close, lest we too become phantoms.'",
                        "*Vern mutters* 'I swore I’d never abandon them again. Not this time.'",
                        "'The sea remembers. And so do I.'"
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
