using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlDevour : XmlAttachment
    {
        private double m_HealPercent = 15.0; // Percent of max health to heal
        private TimeSpan m_Cooldown = TimeSpan.FromMinutes(1); // Cooldown period
        private DateTime m_NextUseTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public double HealPercent { get { return m_HealPercent; } set { m_HealPercent = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlDevour(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlDevour() { }

        [Attachable]
        public XmlDevour(double healPercent, double cooldown)
        {
            HealPercent = healPercent;
            Cooldown = TimeSpan.FromMinutes(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HealPercent);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextUseTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_HealPercent = reader.ReadDouble();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextUseTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
			return String.Format("Devour: Heals a % of max health by consuming nearby corpses. Cooldown: {0} seconds.", Cooldown.TotalSeconds);
        }

        public override void OnAttach()
        {
            base.OnAttach();
            // You can trigger an effect or initialize the ability here if needed.
        }

        public void ConsumeCorpses(Mobile consumer)
        {
            if (DateTime.Now < m_NextUseTime)
            {
                consumer.SendMessage("This ability is on cooldown.");
                return;
            }

            if (consumer is PlayerMobile || consumer is BaseCreature)
            {
                int healAmount = (int)(consumer.HitsMax * (m_HealPercent / 100.0));
                bool consumed = false;

                foreach (Item item in consumer.GetItemsInRange(2)) // Check within 2 tiles.
                {
                    if (item is Corpse)
                    {
                        consumed = true;
                        item.Delete(); // Consume the corpse.
                        break; // For this example, we consume only one corpse.
                    }
                }

                if (consumed)
                {
                    consumer.Hits += healAmount; // Heal the consumer.
					consumer.Say("Consumes Corpses!");
                    consumer.SendMessage("You have consumed a corpse and healed yourself.");
                }
                else
                {
                    consumer.SendMessage("No corpses nearby to consume.");
                }

                m_NextUseTime = DateTime.Now + m_Cooldown; // Set the next use time.
            }
        }

        // Example usage: this method should be called when the ability is activated, e.g., via a command or a special item.
        public void Activate(Mobile consumer)
        {
            ConsumeCorpses(consumer);
        }
    }
}
