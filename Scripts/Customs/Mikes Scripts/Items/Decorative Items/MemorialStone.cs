using System;
using Server;
using Server.Items;
using Server.Network;

public class MemorialStone : Item
{
    private static int[] Graphics = new int[]
    {
        0xA9DE, 0xA9DF, 0xA9E0, 0xA9E1, 0xA9E2, 0xA9E3, 0xA9E4, 0xA9E5, 0xA9E6, 0xA9E7
    };

    [Constructable]
    public MemorialStone() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Memorial Stone";
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

    public MemorialStone(Serial serial) : base(serial)
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
