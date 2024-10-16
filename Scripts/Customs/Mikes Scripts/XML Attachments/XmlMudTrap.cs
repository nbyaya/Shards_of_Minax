using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMudTrap : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Cooldown duration
        private DateTime m_NextMudTrap;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlMudTrap(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlMudTrap() { }

        [Attachable]
        public XmlMudTrap(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextMudTrap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_NextMudTrap = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextMudTrap)
            {
                CreateMudTrap(attacker);
                m_NextMudTrap = DateTime.UtcNow + Refractory; // Reset cooldown
            }
        }

        private void CreateMudTrap(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Creates a pool of sticky mud *");
            owner.PlaySound(0x22F);
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 15);

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
                m.SendLocalizedMessage(1072124, "", 0x206); // You are mired in sticky mud, slowing your movement.
                m.FixedParticles(0x37CC, 1, 10, 0x1F78, 0x496, 0, EffectLayer.Waist);
                int oldDex = m.Dex;
                m.Dex = (int)(m.Dex * 0.5);

                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    m.Dex = oldDex;
                    m.SendLocalizedMessage(1072060, "", 0x206); // You manage to free yourself from the mud.
                });
            }
        }
    }
}
