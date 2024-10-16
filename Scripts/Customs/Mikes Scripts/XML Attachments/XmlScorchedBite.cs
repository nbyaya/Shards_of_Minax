using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlScorchedBite : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for the scorched bite
        private int m_StaminaCost = 20; // Stamina cost for the ability
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(40); // Cooldown time
        private DateTime m_NextScorchedBite;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StaminaCost { get { return m_StaminaCost; } set { m_StaminaCost = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlScorchedBite(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlScorchedBite() { }

        [Attachable]
        public XmlScorchedBite(int damage, int staminaCost, double cooldown)
        {
            Damage = damage;
            StaminaCost = staminaCost;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_StaminaCost);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextScorchedBite);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_StaminaCost = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextScorchedBite = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (defender != null && defender.Alive && DateTime.UtcNow >= m_NextScorchedBite)
            {
                UseScorchedBite(attacker, defender);
            }
        }

        private void UseScorchedBite(Mobile attacker, Mobile target)
        {
            if (target != null && target.Alive)
            {
                target.Damage(m_Damage, attacker);
                target.SendMessage("You feel the searing pain of the fiery bite!");
                target.Stam -= m_StaminaCost;

                attacker.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The creature bites with fiery intensity! *");
                m_NextScorchedBite = DateTime.UtcNow + m_Cooldown; // Set cooldown after use
            }
        }
    }
}
