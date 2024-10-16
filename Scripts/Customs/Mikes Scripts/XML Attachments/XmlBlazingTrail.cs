using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBlazingTrail : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromMinutes(1); // Time between activations
        private DateTime m_NextBlazingSpeed;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlBlazingTrail(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlBlazingTrail() { }

        [Attachable]
        public XmlBlazingTrail(double refractory)
        {
            Refractory = TimeSpan.FromMinutes(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextBlazingSpeed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_NextBlazingSpeed = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Blazing Trail Ability: Leave a trail of fire that damages nearby foes.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextBlazingSpeed)
            {
                UseBlazingSpeed(attacker);
                m_NextBlazingSpeed = DateTime.UtcNow + m_Refractory;
            }
        }

        private void UseBlazingSpeed(Mobile owner)
        {
            if (owner.Combatant != null)
            {
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The creature moves in a blazing trail of fire! *");
                Point3D newLocation = new Point3D(owner.X + Utility.RandomMinMax(-5, 5), owner.Y + Utility.RandomMinMax(-5, 5), owner.Z);
                owner.Location = newLocation;

                // Create fire trail effects
                Effects.SendLocationParticles(EffectItem.Create(newLocation, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);

                foreach (Mobile m in owner.GetMobilesInRange(1))
                {
                    if (m != owner && m != owner.Combatant)
                    {
                        m.SendMessage("You are burned by the blazing trail!");
                        m.Damage(Utility.RandomMinMax(5, 10), owner);
                    }
                }
            }
        }
    }
}
