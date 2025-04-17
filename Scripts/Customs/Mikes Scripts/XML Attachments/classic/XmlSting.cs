using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSting : XmlAttachment
    {
        private int m_ExtraDamage = 20; // Extra damage inflicted to already poisoned targets

        [CommandProperty(AccessLevel.GameMaster)]
        public int ExtraDamage { get { return m_ExtraDamage; } set { m_ExtraDamage = value; } }

        public XmlSting(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSting() { }

        [Attachable]
        public XmlSting(int extraDamage)
        {
            ExtraDamage = extraDamage;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_ExtraDamage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_ExtraDamage = reader.ReadInt();
            }
        }

        public override string OnIdentify(Mobile from)
        {
			return String.Format("Sting: Inflicts 20 extra damage to poisoned targets on hit.");
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (defender.Poisoned)
            {
                // Inflicts extra damage which ignores armor
                defender.Damage(m_ExtraDamage, attacker, true); // true indicates damage should ignore armor
				attacker.Say("Stings!");
            }
        }
    }
}
