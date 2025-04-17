using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceRemoveTrap : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceRemoveTrap()
            : base(SkillName.RemoveTrap, 1.0) // 1.0 skill points for RemoveTrap
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceRemoveTrap(Serial serial)
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
