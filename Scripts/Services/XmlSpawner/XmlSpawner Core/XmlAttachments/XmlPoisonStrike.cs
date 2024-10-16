using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPoisonStrike : XmlAttachment
    {
        private Poison m_PoisonEffect = Poison.Regular;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5);
        private DateTime m_EndTime;
        private TimeSpan m_Duration = TimeSpan.FromSeconds(10); 

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison PoisonEffect { get { return m_PoisonEffect; } set { m_PoisonEffect = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        public XmlPoisonStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlPoisonStrike(Poison poison, double duration)
        {
            m_PoisonEffect = poison;
            Duration = TimeSpan.FromSeconds(duration);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (defender != null && attacker != null)
            {
                defender.SendMessage("You've been poisoned!");
                defender.FixedParticles(0x3E02, 10, 30, 5052, EffectLayer.LeftFoot); // Poison effect
                defender.ApplyPoison(attacker, m_PoisonEffect);
                
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            Poison.Serialize(m_PoisonEffect, writer);
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
            writer.Write(m_Duration);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_PoisonEffect = Poison.Deserialize(reader);
                m_Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
                m_Duration = reader.ReadTimeSpan();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Poison Strike with " + m_PoisonEffect.Name + " for " + Duration.TotalSeconds + " secs. " + Refractory.TotalSeconds + " secs between uses.";
        }
    }
}
