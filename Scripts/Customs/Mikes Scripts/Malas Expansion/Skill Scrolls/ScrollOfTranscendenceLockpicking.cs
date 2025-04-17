using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceLockpicking : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceLockpicking()
            : base(SkillName.Lockpicking, 1.0) // 1.0 skill points for Lockpicking
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceLockpicking(Serial serial)
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
