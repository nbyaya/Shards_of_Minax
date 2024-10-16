using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMelodyOfPeace : XmlAttachment
    {
        private int m_HealAmount = 15; // Amount of health restored
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(40); // Cooldown time
        private DateTime m_NextMelodyOfPeace;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HealAmount { get { return m_HealAmount; } set { m_HealAmount = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlMelodyOfPeace(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlMelodyOfPeace() { }

        [Attachable]
        public XmlMelodyOfPeace(int healAmount, double cooldown)
        {
            HealAmount = healAmount;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HealAmount);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextMelodyOfPeace);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_HealAmount = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextMelodyOfPeace = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Melody of Peace: Heals {m_HealAmount} health with a cooldown of {m_Cooldown.TotalSeconds} seconds.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextMelodyOfPeace)
            {
                PerformMelodyOfPeace(attacker);
                m_NextMelodyOfPeace = DateTime.UtcNow + m_Cooldown;
            }
        }

        public void PerformMelodyOfPeace(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a soothing tune of peace *");
            owner.PlaySound(0x1F3); // Musical sound

            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).IsFriend(owner))
                {
                    m.Heal(m_HealAmount);
                    m.SendMessage("You feel a soothing wave of peace.");
                    m.AddSkillMod(new DefaultSkillMod(SkillName.MagicResist, true, m.Skills[SkillName.MagicResist].Base + 15.0));
                    m.SendMessage("Your defenses are enhanced!");
                }
            }
        }
    }
}
