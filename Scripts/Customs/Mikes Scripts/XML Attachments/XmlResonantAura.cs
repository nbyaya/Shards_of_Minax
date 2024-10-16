using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlResonantAura : XmlAttachment
    {
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(2); // Cooldown for the aura
        private DateTime m_NextResonantAura;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlResonantAura(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlResonantAura() { }

        [Attachable]
        public XmlResonantAura(double cooldown)
        {
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextResonantAura);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Cooldown = reader.ReadTimeSpan();
                m_NextResonantAura = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Resonant Aura: Empowers allies and hinders enemies.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextResonantAura)
            {
                ApplyResonantAura(attacker);
                m_NextResonantAura = DateTime.UtcNow + m_Cooldown;
            }
        }

        private void ApplyResonantAura(Mobile owner)
        {
            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a resonant aura *");
            owner.PlaySound(0x1F5); // Resonant aura sound

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m is BaseCreature && ((BaseCreature)m).IsFriend(owner))
                {
                    m.SendMessage("You are empowered by the resonant aura!");
                    m.AddSkillMod(new DefaultSkillMod(SkillName.MagicResist, true, m.Skills[SkillName.MagicResist].Base + 10.0));
                }
                else if (m != owner)
                {
                    m.SendMessage("You are hindered by the resonant aura!");
                    m.Damage(10, owner);
                    m.SendMessage("You feel your strength sapped by the resonant aura!");
                    m.AddSkillMod(new DefaultSkillMod(SkillName.MagicResist, true, m.Skills[SkillName.MagicResist].Base - 5.0));
                }
            }
        }
    }
}
