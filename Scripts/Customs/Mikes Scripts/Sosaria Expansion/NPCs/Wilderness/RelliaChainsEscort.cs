using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RelliaChainsQuest : BaseQuest
    {
        public override object Title { get { return "Chains of the Past"; } }

        public override object Description
        {
            get
            {
                return 
                    "You see a tall, weathered woman tightening her cloak over old battle scars. She glances around nervously, gripping a curved blade wrapped in cloth.<br><br>" +
                    "\"I need your help. My name is Rellia... once a gladiator of the Crimson Pits. I'm done fighting. This blade is the last piece of my past, and I've a buyer waiting at the East Montor Inn who'll pay well to see it gone. " +
                    "I can't travel alone – too many who remember me would rather see me dead than free. Will you escort me to East Montor Inn? Let me walk the road as a free woman, for once.\"";
            }
        }

        public override object Refuse { get { return "\"I see. Then may the chains of my past hold me no longer than I can bear...\""; } }
        public override object Uncomplete { get { return "\"Still too close to danger... I must reach the inn at East Montor. Please, let’s keep moving.\""; } }

        public RelliaChainsQuest() : base()
        {
            AddObjective(new EscortObjective("East Montor Inn"));
            AddReward(new BaseReward(typeof(WrestlersLeggingsOfBalance), "Wrestlers Leggings Of Balance – a light armor that resists poison and grants stealth."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Rellia exhales, a slow, shaky breath. \"Thank you... This blade’s gone, and so are my chains. Take this, forged from the poison pits I once fought in – may it serve you better than it did me.\"");
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

    public class RelliaChainsEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(RelliaChainsQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBLeatherArmor());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public RelliaChainsEscort() : base()
        {
            Name = "Rellia";
            Title = "of the Chains";
            NameHue = 0x83F;
        }

		public RelliaChainsEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 70, 60);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x8401;
            HairItemID = 0x2049; // Long Hair
            HairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherBustierArms() { Hue = 0x455 });
            AddItem(new LeatherSkirt() { Hue = 0x1BB });
            AddItem(new BodySash() { Hue = 0x3B2 });
            AddItem(new LeatherGloves() { Hue = 0x1BB });
            AddItem(new Boots() { Hue = 0x455 });
            AddItem(new Cloak() { Hue = 0x3B2 });
            AddItem(new Cutlass() { Hue = 0x48F, Movable = false }); // Wrapped blade, part of backstory
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
                        "\"These roads... too many memories. Too much blood.\"",
                        "*Rellia grips the cloth-wrapped blade tightly* \"Soon... soon I will be free.\"",
                        "\"Do you hear that? Footsteps... or just ghosts from the arena?\"",
                        "*She looks to the horizon* \"One last walk. Just one.\"",
                        "\"East Montor... it’s just ahead, right? I can almost taste the air of freedom.\"",
                        "\"If I fall... don’t let them take the blade. Promise me that.\"",
                        "*Rellia exhales sharply* \"No chains, no masters. Never again.\""
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
