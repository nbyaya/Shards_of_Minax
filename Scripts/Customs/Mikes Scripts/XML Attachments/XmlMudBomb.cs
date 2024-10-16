using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMudBomb : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(20);
        private DateTime m_NextMudBomb;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlMudBomb(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlMudBomb() { }

        [Attachable]
        public XmlMudBomb(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextMudBomb);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_NextMudBomb = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextMudBomb)
            {
                PerformMudBomb(attacker, defender);
                m_NextMudBomb = DateTime.UtcNow + m_Refractory; // Reset cooldown
            }
        }

        private void PerformMudBomb(Mobile owner, Mobile target)
        {
            if (target == null || !target.Alive || owner == null)
                return;

            owner.PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Throws a glob of mud *");
            owner.PlaySound(0x145);

            owner.Direction = owner.GetDirectionTo(target);
            owner.MovingEffect(target, 0xF0D, 7, 1, false, false);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (target.Alive)
                {
                    target.Freeze(TimeSpan.FromSeconds(3));
                    target.FixedEffect(0x376A, 9, 32);
                    target.PlaySound(0x201);
                    AOS.Damage(target, owner, Utility.RandomMinMax(10, 15), 100, 0, 0, 0, 0);
                }
            });
        }
    }
}
