namespace Server.Items
{
    public class PlagueRelic : Item
    {
        [Constructable]
        public PlagueRelic() : base(0x2F59) // Example itemID
        {
            Name = "Plague Relic";
            Hue = 0x835;
        }

        public PlagueRelic(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
