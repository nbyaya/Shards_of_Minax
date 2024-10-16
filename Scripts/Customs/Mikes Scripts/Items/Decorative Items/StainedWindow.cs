using System;
using Server;
using Server.Items;
using Server.Network;

public class StainedWindow : Item
{
    private static int[] Graphics = new int[]
    {
        0xAD79, 0xAD7A, 0xAD7B, 0xAD7C, 0xAD7D, 0xAD7E, 0xAD7F, 0xAD80, 0xA9D6, 0xA9D7, 0xA9D8, 0xA9D9, 0xA9DA, 0xA9DB, 0xA9DC, 0xA9DD
    };

    [Constructable]
    public StainedWindow() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Stained Window";
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

    public StainedWindow(Serial serial) : base(serial)
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
