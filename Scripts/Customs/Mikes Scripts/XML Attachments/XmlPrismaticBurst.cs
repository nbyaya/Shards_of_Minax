using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlPrismaticBurst : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown time for the ability
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlPrismaticBurst(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlPrismaticBurst() { }

        [Attachable]
        public XmlPrismaticBurst(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextUse = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Prismatic Burst: Cooldown of {0} seconds.", m_Cooldown.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextUse)
            {
                PerformPrismaticBurst(attacker);
                m_NextUse = DateTime.UtcNow + m_Cooldown; // Reset cooldown
            }
        }

        public void PerformPrismaticBurst(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Emits a dazzling burst of light *");
            owner.PlaySound(0x212);
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x3709, 10, 20);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = owner.Map.GetMobilesInRange(owner.Location, 5);

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
                m.SendLocalizedMessage(500898); // A blinding light explodes in front of you!
                m.Freeze(TimeSpan.FromSeconds(5));
                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bleed, 1075643, 1075644, TimeSpan.FromSeconds(5), m, "50"));
            }
        }
    }
}
