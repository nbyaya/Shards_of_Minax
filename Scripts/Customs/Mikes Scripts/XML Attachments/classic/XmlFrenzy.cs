using System;
using Server;
using Server.Mobiles;
using Server.Items; // Make sure this line is added

namespace Server.Engines.XmlSpawner2
{
    public class XmlFrenzy : XmlAttachment
    {
        private const double ChanceToActivate = 0.15; // 15% chance to activate
        private const int DexBonus = 50; // Dexterity bonus amount
        private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30); // Duration of the effect
        private DateTime m_EndTime; // Tracks when the effect should end

        public XmlFrenzy(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFrenzy()
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

            if (version >= 0)
            {
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            if (from == null || !from.Alive) return null;

            TimeSpan timeLeft = m_EndTime - DateTime.Now;

            if (timeLeft > TimeSpan.Zero)
            {
                return String.Format("Frenzy: Dexterity bonus active for {0} more seconds.", timeLeft.TotalSeconds);
            }
            else
            {
                return "Frenzy: Grants a 15% chance on melee attack to increase dexterity by 50 for 30 seconds.";
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (attacker == null || !attacker.Alive || DateTime.Now < m_EndTime)
                return;

            if (Utility.RandomDouble() < ChanceToActivate)
            {
                attacker.SendMessage("You feel a surge of frenzied energy!");
				attacker.Say("Enters Frenzy!");
                attacker.Dex += DexBonus;
                m_EndTime = DateTime.Now + Duration;

                Timer.DelayCall(Duration, delegate
                {
                    attacker.Dex -= DexBonus;
                    attacker.SendMessage("The frenzied energy fades.");
                });
            }
        }
    }
}
