using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBackStrike : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextPhantomStrike;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlBackStrike(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlBackStrike() { }

        [Attachable]
        public XmlBackStrike(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextPhantomStrike);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                Refractory = reader.ReadTimeSpan();
                m_NextPhantomStrike = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return "Phantom Strike: A chilling attack that strikes from behind.";
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextPhantomStrike && defender != null)
            {
                PerformPhantomStrike(attacker, defender);
                m_NextPhantomStrike = DateTime.UtcNow + m_Refractory; // Recalculate next activation
            }
        }

        private void PerformPhantomStrike(Mobile attacker, Mobile target)
        {
            if (target == null || target.Deleted || attacker.Deleted)
                return;

            Point3D behindLocation = new Point3D(target.X + Utility.RandomMinMax(-1, 1), target.Y + Utility.RandomMinMax(-1, 1), target.Z);
            if (target.Map.CanSpawnMobile(behindLocation))
            {
                attacker.Location = behindLocation;
                target.SendMessage("You feel a sudden chill as a phantom strikes from behind!");
                attacker.AggressiveAction(target);
                target.Damage(Utility.RandomMinMax(20, 30), attacker);
            }
        }
    }
}
