using System;
using Server;

namespace Server.Items
{
    public class PickaxeOfWealth : Pickaxe
    {
        [Constructable]
        public PickaxeOfWealth()
        {
            Name = "Pickaxe of Wealth";
            Hue = 0x501; // Gold hue

            Attributes.Luck = 100;
            Attributes.WeaponDamage = 50;
            SkillBonuses.SetValues(0, SkillName.Mining, 10.0);
        }

        public PickaxeOfWealth(Serial serial) : base(serial)
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
