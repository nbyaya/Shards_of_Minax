using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
    // ---------------------------------------------------
    // Blood Swarm Gem
    // ---------------------------------------------------
    public class BloodSwarmGem : BaseSocketAugmentation
    {
        private bool isActive;
        private Timer m_Timer;
        private int maxPets;

        [Constructable]
        public BloodSwarmGem() : base(0x2809)
        {
            Name = "Blood Swarm Gem";
            Hue = 1358; // Blood red hue
            isActive = false;
        }

        public override int SocketsRequired { get { return 2; } } // Adjust as needed
        
        public override int Icon { get { return 0x2809;} } // Adjust icon as needed

        public override int IconXOffset { get { return 10;} }

        public override int IconYOffset { get { return 10;} }

        public BloodSwarmGem(Serial serial) : base(serial)
        {
        }

        public override string OnIdentify(Mobile from)
        {
            return "Grants the ability to summon a swarm of blood elementals.";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if (target is Item && !(target is Mobile))
            {
                Item item = target as Item;
                maxPets = from.FollowersMax;

                isActive = !isActive;
                if (isActive)
                {
                    from.SendMessage("You activate the Blood Swarm.");
                    StartBloodSwarm(from, item);
                }
                else
                {
                    from.SendMessage("You deactivate the Blood Swarm.");
                    StopBloodSwarm();
                }
                return true;
            }

            return false;
        }

        private void StartBloodSwarm(Mobile from, Item item)
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), () => SummonBloodElemental(from, item));
        }

        private void SummonBloodElemental(Mobile from, Item item)
        {
            if (from.Followers < maxPets)
            {
                BaseCreature elemental = new BloodElemental(); // Assuming BloodElemental is a defined creature
                elemental.Controlled = true;
                elemental.ControlMaster = from;
                elemental.ControlOrder = OrderType.Come;
                elemental.MoveToWorld(from.Location, from.Map);
                from.Followers++;
            }
            else
            {
                from.SendMessage("You have reached the limit of your control.");
                StopBloodSwarm();
            }
        }

        private void StopBloodSwarm()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            isActive = false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(isActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            isActive = reader.ReadBool();
        }
    }
}
