using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMoltenBlast : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown period for the ability
        private DateTime m_NextMoltenBlast;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlMoltenBlast(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlMoltenBlast() { }

        [Attachable]
        public XmlMoltenBlast(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextMoltenBlast);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextMoltenBlast = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Molten Blast Ability: Cooldown of {m_Cooldown.TotalSeconds} seconds.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            PerformMoltenBlast(attacker);
        }

        private void PerformMoltenBlast(Mobile owner)
        {
            if (owner.Combatant != null && DateTime.UtcNow >= m_NextMoltenBlast)
            {
                Mobile target = owner.Combatant as Mobile;

                if (target != null && target.Alive)
                {
                    Point3D location = target.Location;
                    Map map = target.Map;

                    Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 5013);
                    target.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Molten Blast! *");

                    foreach (Mobile m in map.GetMobilesInRange(location, 3))
                    {
                        if (m != owner && m.Player)
                        {
                            AOS.Damage(m, owner, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                        }
                    }

                    m_NextMoltenBlast = DateTime.UtcNow + m_Cooldown; // Set next available time
                }
            }
        }
    }
}
