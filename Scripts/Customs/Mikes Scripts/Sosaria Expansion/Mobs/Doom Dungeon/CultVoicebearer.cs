using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cult voicebearer corpse")]
    public class CultVoicebearer : EvilMage
    {
        [Constructable]
        public CultVoicebearer()
        {
            Name = "Cult Voicebearer";
            Hue = 1175; // Deep violet for uniqueness
            Title = "of the Broken Star";

            SetStr(110, 130);
            SetDex(95, 115);
            SetInt(120, 140);

            SetHits(150, 200);
            SetDamage(10, 15);

            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 25, 35);

            VirtualArmor = 28;

            AddItem(new Robe() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1109 });

            PackReg(8);
            PackItem(new BlackPearl(5));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new BlackSoulstone()); // Rare drop
        }

        public override void OnThink()
        {
            base.OnThink();

            // Unique ability: periodically chant to cause an AOE debuff
            if (DateTime.UtcNow > m_NextChant)
            {
                PublicOverheadMessage(Server.Network.MessageType.Emote, 0x3B2, false, "*chants in tongues of unmaking*");
                Effects.PlaySound(Location, Map, 0x5C9);

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && m.Player)
                        m.SendMessage(0x22, "You feel your thoughts blur under the Voicebearer’s chant!");
                }

                m_NextChant = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
        }

        private DateTime m_NextChant = DateTime.UtcNow + TimeSpan.FromSeconds(10);

        public CultVoicebearer(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
