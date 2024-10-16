using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFrenziedAttack : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(4);
        private DateTime m_EndTime;

        [Attachable]
        public XmlFrenziedAttack() { }

        public XmlFrenziedAttack(ASerial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_EndTime = reader.ReadDeltaTime();
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_EndTime)
            {
                PerformFrenziedAttack(attacker, defender);
                m_EndTime = DateTime.UtcNow + m_Refractory;
            }
        }

        private void PerformFrenziedAttack(Mobile attacker, Mobile defender)
        {
            attacker.PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "* unleashes a frenzied attack! *");
            attacker.PlaySound(0x208); // Roar sound

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    if (defender != null && defender.Alive)
                    {
                        AOS.Damage(defender, attacker, Utility.RandomMinMax(7, 12), 0, 100, 0, 0, 0);
                        defender.SendMessage("You are hit by a rapid series of attacks!");
                    }
                });
            }
        }
    }
}
