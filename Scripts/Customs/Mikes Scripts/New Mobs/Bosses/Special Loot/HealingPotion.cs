using System;
using Server;

namespace Server.Items
{
    public class HealingPotion : BasePotion
    {
        [Constructable]
        public HealingPotion() : base(0xF0C, PotionEffect.Heal)
        {
            Hue = 0x499;
            Name = "Healing Potion";
        }

        public HealingPotion(Serial serial) : base(serial)
        {
        }

        public override void Drink(Mobile from)
        {
            if (from.Poisoned || MortalStrike.IsWounded(from))
            {
                from.SendMessage("You cannot heal while poisoned or mortally wounded.");
                return;
            }

            from.Heal(Utility.RandomMinMax(50, 70)); // Heal a random amount between 50 and 70

            from.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            from.PlaySound(0x1F2);

            this.Consume();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
