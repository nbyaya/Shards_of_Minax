using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSunlitHeal : XmlAttachment
    {
        private int m_HealAmount = 30; // Amount to heal
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown duration
        private DateTime m_NextSunlitHeal;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HealAmount { get { return m_HealAmount; } set { m_HealAmount = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlSunlitHeal(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSunlitHeal() { }

        [Attachable]
        public XmlSunlitHeal(int healAmount, double cooldown)
        {
            HealAmount = healAmount;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HealAmount);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextSunlitHeal);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_HealAmount = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextSunlitHeal = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextSunlitHeal)
            {
                PerformSunlitHeal(attacker);
                m_NextSunlitHeal = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformSunlitHeal(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sunlit Heal! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16); // Healing effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && creature.Alive)
                {
                    creature.Hits = Math.Min(creature.HitsMax, creature.Hits + m_HealAmount); // Heal
                    creature.SendMessage("You feel revitalized by the sun's power!");

                    // Optional buff effect
                    if (m != owner && Utility.RandomDouble() < 0.5)
                    {
                        creature.SendMessage("You are blessed with a protective aura!");
                        creature.FixedEffect(0x37B9, 10, 16); // Buff effect
                        creature.VirtualArmor += 10; // Temporary defense boost

                        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                        {
                            creature.VirtualArmor -= 10; // Revert defense boost
                        });
                    }
                }
            }
        }
    }
}
