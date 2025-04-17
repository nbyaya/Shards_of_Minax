using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceCamping : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceCamping()
            : base(SkillName.Camping, 1.0) // 1.0 skill points for Camping
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceCamping(Serial serial)
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
