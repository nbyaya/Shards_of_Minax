using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceMaceFighting : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceMaceFighting()
            : base(SkillName.Macing, 1.0) // 1.0 skill points for MaceFighting
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceMaceFighting(Serial serial)
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
