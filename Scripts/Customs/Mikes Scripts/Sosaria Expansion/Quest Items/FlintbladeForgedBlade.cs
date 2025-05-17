using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FlintbladeForgedBlade : Longsword
    {
        [Constructable]
        public FlintbladeForgedBlade()
        {
            Name = "Flintblade Forged Blade";
            Hue = 0x4F2; // glowing ember-like hue
            LootType = LootType.Blessed;

            Attributes.WeaponDamage = 25;         // Increased base weapon damage
            Attributes.WeaponSpeed = 10;          // Faster swing
            Attributes.SpellChanneling = 1;       // Allows mages to wield it

            Slayer = SlayerName.ElementalBan;     // Bonus vs. elementals (great for Death Glutch)
            WeaponAttributes.HitFireArea = 25;    // AoE fire burst on hit
            WeaponAttributes.HitLeechHits = 10;   // Minor life leech
        }

        public FlintbladeForgedBlade(Serial serial) : base(serial) { }


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
