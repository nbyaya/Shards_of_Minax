using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlRandomMinionStrike : XmlAttachment
    {
        // Define a list of monster types for MinionStrike
        private static readonly string[] MinionList = new string[]
        {
            "AbysmalHorror",
            "AcidElemental",
            "AgapiteElemental",
            "AirElemental",
            "Alligator",
            "AncientLich",
            "AncientWyrm",
            "AntLion",
            "ArcaneDaemon",
            "ArcherGuard",
            "ArcticOgreLord",
            "BakeKitsune",
            "Balron",
            "Barracoon"
        };

        // Default constructor
        [Attachable]
        public XmlRandomMinionStrike()
        {
        }

        // Constructor with serial, needed for deserialization
        public XmlRandomMinionStrike(ASerial serial) : base(serial)
        {
        }

        // When the attachment is attached to a mobile
        public override void OnAttach()
        {
            base.OnAttach();

            Mobile mob = AttachedTo as Mobile;

            if (mob != null)
            {
                AttachRandomMinionStrike(mob);
                Delete(); // Delete itself after attaching the random minion strike
            }
        }

        // Randomly selects a minion from the list and attaches it as a MinionStrike
        private void AttachRandomMinionStrike(Mobile mob)
        {
            Random random = new Random();
            int index = random.Next(MinionList.Length); // Select a random index

            string minionType = MinionList[index];

            // Attach the random MinionStrike with the selected minion type
            XmlAttachment minionStrike = (XmlAttachment)Activator.CreateInstance(typeof(XmlMinionStrike), minionType);
            XmlAttach.AttachTo(mob, minionStrike);

            mob.SendMessage($"A MinionStrike with a {minionType} has been attached to you!");
        }

        // Required method for serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        // Required method for deserialization
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        // For identifying the attachment
        public override string OnIdentify(Mobile from)
        {
            return "Random MinionStrike Attachment";
        }
    }
}
