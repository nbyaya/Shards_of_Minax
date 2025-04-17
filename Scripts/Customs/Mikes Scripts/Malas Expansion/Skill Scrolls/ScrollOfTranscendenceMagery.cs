using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceMagery : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceMagery()
            : base(SkillName.Magery, 1.0) // 1.0 skill points for Magery
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceMagery(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
