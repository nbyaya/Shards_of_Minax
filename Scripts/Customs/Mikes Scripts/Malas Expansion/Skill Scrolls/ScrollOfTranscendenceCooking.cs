using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceCooking : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceCooking()
            : base(SkillName.Cooking, 1.0) // 1.0 skill points for Cooking
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceCooking(Serial serial)
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
