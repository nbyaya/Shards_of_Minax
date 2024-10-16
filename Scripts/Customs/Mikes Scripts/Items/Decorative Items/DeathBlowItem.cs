using System;
using Server;
using Server.Items;
using Server.Network;

public class DeathBlowItem : Item
{
    private static int[] Graphics = new int[]
    {
        0xAA37, 0xAA38, 0xAA39, 0xAA3A, 0xAA3B, 0xAA3C, 0xAA3D, 0xAA3E, 0xAA3F, 0xAA40, 0xAA41, 0xAA42
    };

    [Constructable]
    public DeathBlowItem() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Deathblow Item";
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with other items
        Weight = 1.0;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You find the art wonderful!");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public DeathBlowItem(Serial serial) : base(serial)
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
