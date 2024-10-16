using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlRage : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(7);
        private DateTime m_EndTime;

        [Attachable]
        public XmlRage() { }

        public XmlRage(ASerial serial) : base(serial) { }

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
                TriggerRage(attacker);
                m_EndTime = DateTime.UtcNow + m_Refractory;
            }
        }

        private void TriggerRage(Mobile attacker)
        {
            if (attacker is BaseCreature creature)
            {
                attacker.PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "* enters a state of uncontrollable fury! *");
                attacker.PlaySound(0x208); // Roar sound

                // Increase damage and decrease defense
                creature.SetDamage(20, 35);
                creature.VirtualArmor = 20;

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    creature.SetDamage(29, 35); // Reset damage after 30 seconds
                    creature.VirtualArmor = 90; // Restore original armor
                });
            }
        }
    }
}
