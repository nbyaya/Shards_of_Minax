
using Server.Items;

namespace Server.FarmingSystem
{
    public class CropSextant : Item
    {
        [Constructable]
        public CropSextant()
            : base(0x1058)
        {
            Weight = 2.0;
            this.Name = "Crop Sextant";
        }

        public CropSextant(Serial serial)
                : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack.Items.Contains(this))
            {
                if (from.Skills[SkillName.Tracking].Base >= 30)
                {

                    Item totrack = null;

                    foreach (Item item in World.Items.Values)
                    {
                        if (item is BaseFarmingSeed && ((BaseFarmingSeed)item).Owner == from.Serial)
                        {
                            totrack = item;
                        }
                        else if (item is BaseFarmingCrop && ((BaseFarmingCrop)item).Owner == from.Serial)
                        {
                            totrack = item;
                        }
                    }

                    if (totrack != null)
                    {
                        if (from.Map == totrack.Map)
                        {
                            from.QuestArrow = new SkillHandlers.TrackArrow(from, totrack, 1000);

                            string message = Sextant.GetCoords(totrack);

                            from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "Crop Location : " + message);
                        }
                        else
                        {
                            from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "Your crop is in " + totrack.Map.ToString() + ". Go to that map and then use the crop sextant.");
                        }
                    }
                    else
                    {
                        from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "You do not have a crop planted.");
                    }
                }
                else
                {
                    from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "You need at least 30 tracking to use this. Visit a skill trainer or gain some skill and try again.");
                }
            }
            else
            {
                from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "This must be in your backpack to use it.");
            }
        }
    }
}
