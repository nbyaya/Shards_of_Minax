using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlLavaFlow : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for the Lava Flow
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown time for the ability
        private DateTime m_NextLavaFlow;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlLavaFlow(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlLavaFlow() { }

        [Attachable]
        public XmlLavaFlow(int damage, double cooldown)
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
            writer.WriteDeltaTime(m_NextLavaFlow);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextLavaFlow = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Lava Flow: Deals {m_Damage} damage to targets within 3 tiles every {m_Cooldown.TotalSeconds} seconds.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextLavaFlow)
            {
                PerformLavaFlow(attacker);
                m_NextLavaFlow = DateTime.UtcNow + m_Cooldown; // Reset cooldown
            }
        }

        private void PerformLavaFlow(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Unleashes a flow of molten lava *");
            owner.PlaySound(0x241);
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x36B0, 10, 30);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = owner.GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (m != owner && owner.CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                DoLavaFlowDamage(m);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), 4, new TimerStateCallback(LavaFlowDamage), targets);
        }

        private void LavaFlowDamage(object state)
        {
            List<Mobile> targets = (List<Mobile>)state;

            foreach (Mobile m in targets)
            {
                if (m.Alive && m.Map != null && m.Map == m.Map && m.InRange(m, 3))
                {
                    DoLavaFlowDamage(m);
                }
            }
        }

        private void DoLavaFlowDamage(Mobile target)
        {
            if (target != null && target.Alive)
            {
                target.Damage(m_Damage, target); // Damage the target
            }
        }
    }
}
