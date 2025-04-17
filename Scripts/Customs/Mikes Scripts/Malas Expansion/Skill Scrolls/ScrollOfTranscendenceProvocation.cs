using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceProvocation : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceProvocation()
            : base(SkillName.Provocation, 1.0) // 1.0 skill points for Provocation
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceProvocation(Serial serial)
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
