using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDreamyAura : XmlAttachment
    {
        private int m_Damage = 5; // Damage dealt to targets
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(40); // Cooldown duration
        private DateTime m_NextDreamyAura;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlDreamyAura(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlDreamyAura() { }

        [Attachable]
        public XmlDreamyAura(int damage, double cooldown)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextDreamyAura);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextDreamyAura = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextDreamyAura)
            {
                ApplyDreamyAura(attacker);
                m_NextDreamyAura = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void ApplyDreamyAura(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The aura of dreams expands! *");
            Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x373A, 10, 30, 1154);

            foreach (Mobile target in owner.GetMobilesInRange(5))
            {
                if (target != owner && target.Alive && target.Player)
                {
                    target.SendMessage("The aura makes you feel disoriented and weak.");
                    target.Damage(m_Damage, owner);
                    target.Freeze(TimeSpan.FromSeconds(3));
                }
            }
        }
    }
}
