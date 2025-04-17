using Server.Mobiles;

namespace Server.Items
{
    public class ScrollOfTranscendenceItemID : ScrollOfTranscendence
    {
        [Constructable]
        public ScrollOfTranscendenceItemID()
            : base(SkillName.ItemID, 1.0) // 1.0 skill points for ItemID
        {
            Hue = 0x490; // Optional: Customize the hue
        }

        public ScrollOfTranscendenceItemID(Serial serial)
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
