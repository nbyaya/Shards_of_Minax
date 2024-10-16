using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlChaoticSurge : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown for the ability
        private DateTime m_NextChaoticSurge;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlChaoticSurge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlChaoticSurge() { }

        [Attachable]
        public XmlChaoticSurge(double cooldown)
        {
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextChaoticSurge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextChaoticSurge = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextChaoticSurge)
            {
                PerformChaoticSurge(attacker);
                m_NextChaoticSurge = DateTime.UtcNow + m_Cooldown; // Set next activation time
            }
        }

        private void PerformChaoticSurge(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile target in owner.GetMobilesInRange(5))
            {
                if (target != owner && target.Player)
                {
                    int effect = Utility.Random(3);
                    switch (effect)
                    {
                        case 0:
                            target.SendMessage("You are scorched by a burst of fire!");
                            target.Damage(Utility.RandomMinMax(10, 15), owner);
                            break;
                        case 1:
                            target.SendMessage("You are chilled by a blast of ice!");
                            target.Freeze(TimeSpan.FromSeconds(3));
                            break;
                        case 2:
                            target.SendMessage("You feel a wave of poisonous energy!");
                            target.Damage(Utility.RandomMinMax(5, 10), owner);
                            break;
                    }
                }
            }
        }
    }
}
