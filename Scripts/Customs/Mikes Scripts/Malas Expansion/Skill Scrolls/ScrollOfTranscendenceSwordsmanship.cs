using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceSwordsmanship : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceSwordsmanship()
            : base(SkillName.Swords, 1.0) // 1.0 skill points for Swordsmanship
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceSwordsmanship(Serial serial)
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
