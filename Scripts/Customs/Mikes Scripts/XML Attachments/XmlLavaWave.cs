using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlLavaWave : XmlAttachment
    {
        private int m_MinDamage = 10; // Minimum damage of the lava wave
        private int m_MaxDamage = 20; // Maximum damage of the lava wave
        private TimeSpan m_ActivationTime = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextActivation;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan ActivationTime { get { return m_ActivationTime; } set { m_ActivationTime = value; } }

        public XmlLavaWave(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlLavaWave() { }

        [Attachable]
        public XmlLavaWave(int minDamage, int maxDamage, double activationTime)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            ActivationTime = TimeSpan.FromSeconds(activationTime);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_ActivationTime);
            writer.WriteDeltaTime(m_NextActivation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_ActivationTime = reader.ReadTimeSpan();
                m_NextActivation = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextActivation)
            {
                PerformLavaWave(attacker);
                m_NextActivation = DateTime.UtcNow + m_ActivationTime;
            }
        }

        private void PerformLavaWave(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Unleashes a wave of lava *");
            owner.PlaySound(0x108);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    Point3D loc = new Point3D(owner.X + Utility.RandomMinMax(-1, 1), owner.Y + Utility.RandomMinMax(-1, 1), owner.Z);
                    Effects.SendLocationEffect(loc, owner.Map, 0x36B0, 20, 10, 0, 5);

                    foreach (Mobile m in owner.GetMobilesInRange(1))
                    {
                        if (m != owner && owner.CanBeHarmful(m))
                        {
                            owner.DoHarmful(m);
                            int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
                            AOS.Damage(m, owner, damage, 0, 100, 0, 0, 0);
                            m.SendLocalizedMessage(1008112); // The intense heat is damaging you!

                            // Start burning effect
                            new BurningTimer(m, TimeSpan.FromSeconds(5)).Start();
                        }
                    }
                });
            }
        }
    }
}
