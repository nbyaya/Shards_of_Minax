using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.SkillHandlers;

namespace Server.Items
{
    public class CampersBagOfHolding : Bag
    {
        private const double RequiredCampingSkill = 50.0;

        [Constructable]
        public CampersBagOfHolding() : this(4) // Default to 24 item capacity
        {
        }

        [Constructable]
        public CampersBagOfHolding(int maxitems) : base()
        {
            Weight = 1.0;
            MaxItems = maxitems;
            Name = "Camper's Bag of Holding";
            Hue = 1923;
            LootType = LootType.Blessed;
        }

        public CampersBagOfHolding(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            // Check if the player has enough Camping skill
            if (from.Skills[SkillName.Camping].Base >= RequiredCampingSkill)
            {
                base.OnDoubleClick(from); // Open the bag
            }
            else
            {
                from.SendMessage("You lack the required Camping skill (50.0) to use this bag.");
            }
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

        public override int GetTotal(TotalType type)
        {
            if (type != TotalType.Weight)
                return base.GetTotal(type);
            else
                return (int)(TotalItemWeights() * 0.0); // No weight
        }

        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            if (type != TotalType.Weight)
                base.UpdateTotal(sender, type, delta);
            else
                base.UpdateTotal(sender, type, (int)(delta * 0.0)); // No weight change
        }

        private double TotalItemWeights()
        {
            double weight = 0.0;
            foreach (Item item in Items)
                weight += (item.Weight * item.Amount);
            return weight;
        }
    }
}
