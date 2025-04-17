using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendencePoisoning : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendencePoisoning()
            : base(SkillName.Poisoning, 1.0) // 1.0 skill points for Poisoning
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendencePoisoning(Serial serial)
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
