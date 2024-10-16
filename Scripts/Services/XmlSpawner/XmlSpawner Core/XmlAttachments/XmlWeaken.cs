using System;
using Server;
using Server.Mobiles;
using Server.Items; // Make sure this line is added
using Server.Network; // Required for MessageType

namespace Server.Engines.XmlSpawner2
{
    public class XmlWeaken : XmlAttachment
    {
        private TimeSpan m_Duration = TimeSpan.FromSeconds(10); // Duration of the effect
        private DateTime m_LastActivated;

        public XmlWeaken(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlWeaken() { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_LastActivated);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            if (version >= 0)
            {
                m_LastActivated = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Weaken: Reduces target's STR, DEX, and INT by 10 for 10 seconds. 15% chance on attack.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (attacker == null || defender == null)
                return;

            // Check for the 15% chance to activate
            if (Utility.RandomDouble() <= 0.15)
            {
                if (DateTime.Now - m_LastActivated > TimeSpan.FromSeconds(0.5)) // Prevents rapid re-triggering
                {
                    // Apply the debuff
                    defender.Str -= 10;
                    defender.Dex -= 10;
                    defender.Int -= 10;
					defender.Say("Weakned!");

                    // Schedule the removal of the debuff
                    Timer.DelayCall(m_Duration, () =>
                    {
                        defender.Str += 10;
                        defender.Dex += 10;
                        defender.Int += 10;
                    });

                    m_LastActivated = DateTime.Now;
                }
            }
        }
    }
}
