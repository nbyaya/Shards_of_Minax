using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBackstab : XmlAttachment
    {
        private bool m_Activated = false; // Tracks whether the next attack is a backstab
        private DateTime m_LastStealthCheck;
        private TimeSpan m_CheckInterval = TimeSpan.FromSeconds(5); // Interval to check stealth status

        [Attachable]
        public XmlBackstab() { }

        // ✅ Correct constructor for XmlSpawner2
        public XmlBackstab(ASerial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Activated);
            writer.WriteDeltaTime(m_LastStealthCheck);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Activated = reader.ReadBool();
                m_LastStealthCheck = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Backstab: Increases the damage of the next melee attack if stealthed.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (!(attacker is BaseCreature || attacker is PlayerMobile))
                return;

            // Check if we should re-evaluate stealth status
            if (DateTime.Now - m_LastStealthCheck > m_CheckInterval)
            {
                m_Activated = attacker.Hidden; // Update activation status based on stealth
                m_LastStealthCheck = DateTime.Now;
            }

            if (m_Activated)
            {
                // Apply backstab effect
                int additionalDamage = (int)(damageGiven * 1.5); // 150% damage increase
                defender.Damage(additionalDamage, attacker);
                attacker.Say("Backstab!");

                // Only trigger once, then require re-stealthing
                m_Activated = false;
            }
        }
    }
}
