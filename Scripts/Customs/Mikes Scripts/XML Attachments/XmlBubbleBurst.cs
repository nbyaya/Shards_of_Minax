using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBubbleBurst : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(15); // Time between activations
        private DateTime m_NextBubbleBurst;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlBubbleBurst(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlBubbleBurst() { }

        [Attachable]
        public XmlBubbleBurst(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextBubbleBurst);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_NextBubbleBurst = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Bubble Burst: Deals cold damage and knocks back enemies.");
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextBubbleBurst)
            {
                PerformBubbleBurst(attacker);
                m_NextBubbleBurst = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformBubbleBurst(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bubble Burst *");
            owner.PlaySound(0x026); // Bubble pop sound

            foreach (Mobile m in owner.GetMobilesInRange(3))
            {
                if (m != owner && m.Alive && owner.CanBeHarmful(m))
                {
                    owner.DoHarmful(m);
                    AOS.Damage(m, owner, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0); // Pure cold damage
                    m.SendLocalizedMessage(1114727); // The burst of bubbles knocks you back!
                    m.MovingParticles(owner, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0); // Spray of water particles
                }
            }
        }
    }
}
