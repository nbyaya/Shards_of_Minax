using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlIllusionAbility : XmlAttachment
    {
        private double m_Chance = 0.25; // Probability of creating an illusion
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(2); // Cooldown for ability
        private DateTime m_NextIllusion;

        [CommandProperty(AccessLevel.GameMaster)]
        public double Chance { get { return m_Chance; } set { m_Chance = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlIllusionAbility(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlIllusionAbility() { }

        [Attachable]
        public XmlIllusionAbility(double chance, TimeSpan cooldown)
        {
            Chance = chance;
            Cooldown = cooldown;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Chance);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextIllusion);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Chance = reader.ReadDouble();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextIllusion = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextIllusion && Utility.RandomDouble() < m_Chance)
            {
                if (defender == null || defender.Map == null)
                return;
				
				defender.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* An illusionary duplicate appears! *");
                BaseCreature illusion = new DreamyFerret { Hue = 1154 }; // Create the illusion with a different hue
                illusion.MoveToWorld(attacker.Location, attacker.Map);
                illusion.Controlled = false;
                illusion.Blessed = true;

                Timer.DelayCall(TimeSpan.FromMinutes(1), () =>
                {
                    if (!illusion.Deleted)
                        illusion.Delete();
                });

                m_NextIllusion = DateTime.UtcNow + m_Cooldown; // Reset cooldown
            }
        }
    }
}
