using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTrickstersGambit : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromMinutes(0.1); // Default cooldown for Gambit
        private DateTime m_NextUse;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlTrickstersGambit(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTrickstersGambit() { }

        [Attachable]
        public XmlTrickstersGambit(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.Write(m_NextUse);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Refractory = reader.ReadTimeSpan();
            m_NextUse = reader.ReadDateTime();
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Boosts strength and dexterity by 20 for 30 seconds every {0} seconds.", m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_NextUse)
            {
                TrickstersGambit(attacker);
                m_NextUse = DateTime.Now + m_Refractory;
            }
        }

        private void TrickstersGambit(Mobile owner)
        {
            if (owner == null)
                return;

            owner.AddStatMod(new StatMod(StatType.Str, "TrickstersGambitStr", 20, TimeSpan.FromSeconds(30)));
            owner.AddStatMod(new StatMod(StatType.Dex, "TrickstersGambitDex", 20, TimeSpan.FromSeconds(30)));
        }
    }
}
