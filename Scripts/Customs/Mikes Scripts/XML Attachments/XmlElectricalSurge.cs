using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlElectricalSurge : XmlAttachment
    {
        private bool m_IsSurging;
        private int m_DamageBoost = 10;
        private int m_StrBoost = 30;
        private int m_DexBoost = 20;
        private int m_IntBoost = 10;
        private TimeSpan m_SurgeDuration = TimeSpan.FromSeconds(15);
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsSurging { get { return m_IsSurging; } set { m_IsSurging = value; } }

        public XmlElectricalSurge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlElectricalSurge() { }

        [Attachable]
        public XmlElectricalSurge(double duration, int damageBoost, int strBoost, int dexBoost, int intBoost)
        {
            m_SurgeDuration = TimeSpan.FromSeconds(duration);
            m_DamageBoost = damageBoost;
            m_StrBoost = strBoost;
            m_DexBoost = dexBoost;
            m_IntBoost = intBoost;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_IsSurging);
            writer.Write(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsSurging = reader.ReadBool();
            m_EndTime = reader.ReadDateTime();
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (!m_IsSurging)
            {
                StartElectricalSurge(attacker);
            }
            else
            {
                ApplySurgeEffects(attacker);
            }
        }

        private void StartElectricalSurge(Mobile owner)
        {
            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Electrical Surge! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16);
            owner.PlaySound(0x29F);

            m_IsSurging = true;

            // Boosting raw attributes instead of SetDamage
            if (owner is BaseCreature creature)
            {
                creature.DamageMin += m_DamageBoost;
                creature.DamageMax += m_DamageBoost;
            }

            owner.RawStr += m_StrBoost;
            owner.RawDex += m_DexBoost;
            owner.RawInt += m_IntBoost;

            m_EndTime = DateTime.Now + m_SurgeDuration;

            Timer.DelayCall(m_SurgeDuration, EndElectricalSurge, owner);
        }

        private void EndElectricalSurge(Mobile owner)
        {
            m_IsSurging = false;

            if (owner is BaseCreature creature)
            {
                creature.DamageMin -= m_DamageBoost;
                creature.DamageMax -= m_DamageBoost;
            }

            owner.RawStr -= m_StrBoost;
            owner.RawDex -= m_DexBoost;
            owner.RawInt -= m_IntBoost;
        }

        private void ApplySurgeEffects(Mobile owner)
        {
            foreach (Mobile m in owner.GetMobilesInRange(3))
            {
                if (m != owner && m.Alive)
                {
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.SendMessage("The surge of electricity makes your skin tingle!");
                        m.Damage(5, owner);
                    }
                }
            }
        }
    }
}
