using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceSnooping : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceSnooping()
            : base(SkillName.Snooping, 1.0) // 1.0 skill points for Snooping
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceSnooping(Serial serial)
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
