using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;
using System.Collections.Generic;

public class RandomMagicWeapon : Item
{
    [Constructable]
    public RandomMagicWeapon() : base(0x1F14) // Use a generic item ID (you can change it if desired)
    {
        Name = "Random Magic Weapon";
        LootType = LootType.Blessed; // adjust as needed for your loot system

        // Schedule the transformation to occur shortly after the item spawns.
        Timer.DelayCall(TimeSpan.FromSeconds(0.1), new TimerCallback(TransformWeapon));
    }

    public RandomMagicWeapon(Serial serial) : base(serial) { }

    private void TransformWeapon()
    {
        // Check if the item has not been deleted already.
        if (this.Deleted)
            return;

        // Create the actual weapon using the factory.
        BaseWeapon weapon = RandomMagicWeaponFactory.CreateRandomMagicWeapon();

        // If this placeholder is in a container, add the weapon there.
        if (this.Parent is Container container)
        {
            container.AddItem(weapon);
        }
        else
        {
            // Otherwise, place the weapon in the world at the same location.
            weapon.MoveToWorld(this.Location, this.Map);
        }

        // Delete the placeholder so the player never sees it.
        this.Delete();
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

        // In case the item is reloaded, schedule the transformation again.
        Timer.DelayCall(TimeSpan.FromSeconds(0.1), new TimerCallback(TransformWeapon));
    }
}
