using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class KessaRootbinderQuest : BaseQuest
    {
        public override object Title { get { return "The Fragrance of Dreams"; } }

        public override object Description
        {
            get
            {
                return 
                    "Oh, kind soul! Please, I need your help! I'm Kessa Rootbinder, an herbalist from Yew. I carry rare, volatile dreambloom plants destined for a mystic at the West Montor Inn. They awaken visions... but if jostled, they can cause hallucinations or worse. I can't risk traveling alone—will you escort me safely? I'll reward you with a special Blueberry Pie—an old family recipe that heals and cures.";
            }
        }

        public override object Refuse { get { return "I understand, but beware... these woods stir strange dreams when left unattended."; } }
        public override object Uncomplete { get { return "The dreamblooms are restless... Please, we must hurry to West Montor Inn."; } }

        public KessaRootbinderQuest() : base()
        {
            AddObjective(new EscortObjective("West Montor Inn"));
            AddReward(new BaseReward(typeof(BlueberryPie), "Blueberry Pie"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you for your care. The dreamblooms are safe, and so am I. Please, take this pie—it will soothe body and soul.", null, 0x59B);
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

    public class KessaRootbinderEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(KessaRootbinderQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public KessaRootbinderEscort() : base()
        {
            Name = "Kessa Rootbinder";
            Title = "the Dreambloom Herbalist";
            NameHue = 0x83F;
        }

		public KessaRootbinderEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(40, 40, 30);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47F;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 0x59C }); // Dreamy herbalist gown
            AddItem(new Cloak() { Hue = 0x59B }); // Soft, protective travel cloak
            AddItem(new Sandals() { Hue = 0x481 }); // Simple, earth-toned sandals
            AddItem(new FlowerGarland() { Hue = 0x59C }); // Woven flower garland

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
                        "*Kessa adjusts her pouch nervously* 'The dreamblooms stir... Can you smell them?'",
                        "'I mustn’t let them fall into the wrong hands. The dreams they bring are... powerful.'",
                        "*She hums softly* 'This path feels familiar. Like something from a dream I once had.'",
                        "'Please, be careful where you step—the blooms don’t take kindly to sudden jostles.'",
                        "*Kessa glances at the trees* 'I once saw their leaves shimmer like these... in my sleep.'",
                        "'We’re close, I can feel the inn’s hearth calling. The mystic awaits.'",
                        "*She clutches her pouch tightly* 'Do not listen if the plants begin to whisper... they test the mind.'"
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
