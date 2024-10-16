using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlVolcanicEruption : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(25); // Time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlVolcanicEruption(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlVolcanicEruption() { }

        [Attachable]
        public XmlVolcanicEruption(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Erupts in a burst of molten lava, damaging nearby foes.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_EndTime)
            {
                PerformVolcanicEruption(attacker);
                m_EndTime = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformVolcanicEruption(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Erupts in a burst of molten lava *");
            owner.PlaySound(0x5CF);
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
                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(m, owner, damage, 0, 100, 0, 0, 0);
                m.PlaySound(0x1DD);
            }
        }
    }
}
