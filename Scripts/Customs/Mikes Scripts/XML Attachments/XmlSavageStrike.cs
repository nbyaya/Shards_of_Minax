using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSavageStrike : XmlAttachment
    {
        private int m_Damage = 30;
        private int m_BleedDuration = 10; // Bleed duration in seconds
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(5);
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BleedDuration { get { return m_BleedDuration; } set { m_BleedDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlSavageStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSavageStrike() { }

        public override string OnIdentify(Mobile from)
        {
            return $"Savage Strike: Deals {m_Damage} damage and causes bleeding for {m_BleedDuration} seconds.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextUse)
            {
                PerformSavageStrike(attacker, defender);
                m_NextUse = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformSavageStrike(Mobile attacker, Mobile target)
        {
            if (target != null)
            {
                attacker.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "*Strikes with savage fury!*");
                AOS.Damage(target, attacker, m_Damage, 100, 0, 0, 0, 0);
                target.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "You are bleeding!");

                Timer.DelayCall(TimeSpan.FromSeconds(1), () => BleedEffect(target));
            }
        }

        private void BleedEffect(Mobile target)
        {
            if (target != null && !target.Deleted)
            {
                for (int i = 0; i < m_BleedDuration; i++)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i), () => AOS.Damage(target, null, Utility.RandomMinMax(5, 10), 100, 0, 0, 0, 0));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_Damage);
            writer.Write(m_BleedDuration);
            writer.Write(m_Cooldown);
            writer.Write(m_NextUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Damage = reader.ReadInt();
            m_BleedDuration = reader.ReadInt();
            m_Cooldown = reader.ReadTimeSpan();
            m_NextUse = reader.ReadDateTime();
        }
    }
}
