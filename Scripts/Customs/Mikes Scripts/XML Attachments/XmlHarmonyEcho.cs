using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlHarmonyEcho : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(50);
        private DateTime m_NextHarmonyEcho;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlHarmonyEcho(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlHarmonyEcho() { }

        [Attachable]
        public XmlHarmonyEcho(double cooldown)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextHarmonyEcho);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextHarmonyEcho = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextHarmonyEcho)
            {
                PerformHarmonyEcho(attacker);
                m_NextHarmonyEcho = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void PerformHarmonyEcho(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a harmonious echo *");
            owner.PlaySound(0x1F4); // Musical sound

            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m != owner && m.Combatant != owner)
                {
                    m.SendMessage("The echo confuses you!");
                    m.Freeze(TimeSpan.FromSeconds(3)); // Example effect
                    m.SendMessage("You are dazed and confused!");
                    m.SendMessage("Your weapon feels heavy and unwieldy!");

                    if (m is PlayerMobile player && player.FindItemOnLayer(Layer.OneHanded) != null)
                    {
                        player.SendMessage("Your weapon has been disarmed!");
                        player.EquipItem(player.FindItemOnLayer(Layer.OneHanded));
                        player.SendMessage("You have been disarmed by the echo!");
                    }
                }
            }
        }
    }
}
