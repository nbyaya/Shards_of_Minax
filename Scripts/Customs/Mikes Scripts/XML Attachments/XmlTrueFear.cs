using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTrueFear : XmlAttachment
    {
        private static readonly TimeSpan TrueFearCooldown = TimeSpan.FromSeconds(30);
        private DateTime m_NextTrueFearAllowed;
        private int m_FearDuration = 13;

        [CommandProperty(AccessLevel.GameMaster)]
        public int FearDuration { get { return m_FearDuration; } set { m_FearDuration = value; } }

        [Attachable]
        public XmlTrueFear() { }

        [Attachable]
        public XmlTrueFear(int fearDuration)
        {
            FearDuration = fearDuration;
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (defender != null && DateTime.UtcNow >= m_NextTrueFearAllowed)
            {
                ApplyTrueFear(defender);
                m_NextTrueFearAllowed = DateTime.UtcNow + TrueFearCooldown;
            }
        }

        private void ApplyTrueFear(Mobile target)
        {
            int fearDuration = Math.Max(1, FearDuration - (int)(target.Skills[SkillName.MagicResist].Value / 10.0));
            target.SendMessage("You feel an overwhelming sense of fear!");
            target.Frozen = true;

            BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.TrueFear, 1153791, 1153827, TimeSpan.FromSeconds(fearDuration), target));

            Timer.DelayCall(TimeSpan.FromSeconds(fearDuration), () =>
            {
                target.Frozen = false;
                target.SendMessage("You can move again!");
            });
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_FearDuration);
            writer.WriteDeltaTime(m_NextTrueFearAllowed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_FearDuration = reader.ReadInt();
            m_NextTrueFearAllowed = reader.ReadDeltaTime();
        }
    }
}
