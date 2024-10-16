using System;
using Server;
using Server.Mobiles;
using Server.Items; // Make sure this line is added

namespace Server.Engines.XmlSpawner2
{
    public class XmlEnrage : XmlAttachment
    {
        private const double ChanceToTrigger = 0.15; // 15% chance to trigger
        private const int BonusDamage = 20; // Increase melee damage by 20
        private const int EffectDuration = 30; // Duration in seconds
        private DateTime m_EndTime;

        public XmlEnrage(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlEnrage()
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_EndTime = reader.ReadDeltaTime();
        }

        public override string OnIdentify(Mobile from)
        {
            if (DateTime.Now < m_EndTime)
            {
				return String.Format("Enrage: Increases melee damage by 20.");
            }
            return "Enrage: Increases melee damage by 20 when activated.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            // Check if already enraged or if the ability should trigger
            if (DateTime.Now < m_EndTime || Utility.RandomDouble() > ChanceToTrigger)
                return;

            m_EndTime = DateTime.Now + TimeSpan.FromSeconds(EffectDuration);

            // Apply bonus damage
            weapon.Attributes.WeaponDamage += BonusDamage;
			attacker.Say("Becomes Enraged!");

            // Schedule removal of bonus after duration
            Timer.DelayCall(TimeSpan.FromSeconds(EffectDuration), () =>
            {
                weapon.Attributes.WeaponDamage -= BonusDamage;
            });

            // Optionally, send a message or apply an effect to indicate activation
            attacker.SendMessage("You feel enraged, increasing your melee damage!");
        }
    }
}
