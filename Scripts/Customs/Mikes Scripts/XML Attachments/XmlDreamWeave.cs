using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDreamWeave : XmlAttachment
    {
        private int m_HealAmount = 15; // Amount of health to heal
        private TimeSpan m_Duration = TimeSpan.FromSeconds(45); // Duration of the stat boost
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(45); // Cooldown duration
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HealAmount { get { return m_HealAmount; } set { m_HealAmount = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlDreamWeave(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlDreamWeave() { }

        [Attachable]
        public XmlDreamWeave(int healAmount, double refractory, double duration)
        {
            HealAmount = healAmount;
            Refractory = TimeSpan.FromSeconds(refractory);
            Duration = TimeSpan.FromSeconds(duration);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HealAmount);
            writer.Write(m_Duration);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_HealAmount = reader.ReadInt();
                m_Duration = reader.ReadTimeSpan();
                m_Refractory = reader.ReadTimeSpan();
                m_NextUse = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Dream Weave: Heals {0} health and boosts stats for {1} seconds.", m_HealAmount, m_Duration.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextUse)
            {
                PerformDreamWeave(attacker);
                m_NextUse = DateTime.UtcNow + m_Refractory;
            }
        }

        private void PerformDreamWeave(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The creature weaves a comforting illusion! *");
            Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 1154);

            foreach (Mobile target in owner.GetMobilesInRange(5))
            {
                if (target != owner && target.Alive)
                {
                    target.SendMessage("You feel a soothing aura that heals your wounds and strengthens your resolve.");
                    target.Heal(m_HealAmount);
                    target.SendMessage("You feel stronger and more resilient!");
                    target.AddStatMod(new StatMod(StatType.Str, "Dream Weave", 10, m_Duration));
                    target.AddStatMod(new StatMod(StatType.Dex, "Dream Weave", 10, m_Duration));
                    target.AddStatMod(new StatMod(StatType.Int, "Dream Weave", 10, m_Duration));
                }
            }
        }
    }
}
