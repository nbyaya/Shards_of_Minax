namespace Server.Items
{
    public class PlagueTalisman : Item
    {
        [Constructable]
        public PlagueTalisman() : base(0x2F59) // Example itemID
        {
            Name = "Plague Talisman";
            Hue = 0x835;
        }

        public PlagueTalisman(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); reader.ReadInt(); }
    }
}
