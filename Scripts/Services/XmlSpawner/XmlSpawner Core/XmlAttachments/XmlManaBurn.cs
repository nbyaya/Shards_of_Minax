using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlManaBurn : XmlAttachment
    {
        private int m_MaxManaDrain = 10;  // maximum mana that can be drained
        private double m_DamageMultiplier = 1.5;  // damage will be this multiplier times the mana drained
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10);
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxManaDrain { get { return m_MaxManaDrain; } set { m_MaxManaDrain = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public double DamageMultiplier { get { return m_DamageMultiplier; } set { m_DamageMultiplier = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlManaBurn(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlManaBurn(int maxManaDrain, double damageMultiplier)
        {
            m_MaxManaDrain = maxManaDrain;
            m_DamageMultiplier = damageMultiplier;
        }

        [Attachable]
        public XmlManaBurn(int maxManaDrain, double damageMultiplier, double refractory)
        {
            m_MaxManaDrain = maxManaDrain;
            m_DamageMultiplier = damageMultiplier;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime || defender == null || attacker == null) return;

            int manaToDrain = Math.Min(defender.Mana, m_MaxManaDrain);
            int damage = (int)(manaToDrain * m_DamageMultiplier);

            defender.Mana -= manaToDrain;
            defender.Damage(damage, attacker);

            defender.SendMessage("Your mana has been burned!");
            m_EndTime = DateTime.Now + Refractory;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);  // version
            writer.Write(m_MaxManaDrain);
            writer.Write(m_DamageMultiplier);
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MaxManaDrain = reader.ReadInt();
                m_DamageMultiplier = reader.ReadDouble();
                Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Mana Burn: Drains up to " + m_MaxManaDrain + " mana and deals " + m_DamageMultiplier + "x damage based on drained mana. " +
                   Refractory.TotalSeconds + " secs between uses";
        }
    }
}
