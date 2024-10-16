using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDisarm : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10); // 10 seconds default time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlDisarm(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlDisarm()
        {
        }

        [Attachable]
        public XmlDisarm(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now < m_EndTime) return;

            if (defender != null && attacker != null)
            {
                DisarmTarget(defender);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        private void DisarmTarget(Mobile target)
        {
            if (target == null) return;

            BaseWeapon weapon = target.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;

            if (weapon == null)
                weapon = target.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;

            if (weapon != null)
            {
                target.SendMessage("You've been disarmed!");
                target.PlaySound(0x3B9);  // play a sound for the disarm
                target.AddToBackpack(weapon);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Refractory = reader.ReadTimeSpan();
                TimeSpan remaining = reader.ReadTimeSpan();
                m_EndTime = DateTime.Now + remaining;
            }
        }

        public override string OnIdentify(Mobile from)
        {
            if (Refractory > TimeSpan.Zero)
            {
                return "Disarm - " + Refractory.TotalSeconds + " secs between uses";
            }
            else
            {
                return "Disarm";
            }
        }
    }
}
