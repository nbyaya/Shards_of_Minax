using System;
using Server;
using Server.Items;
using Server.Network;

public class VirtueRune : Item
{
    private static int[] Graphics = new int[]
    {
        0xA517, 0xA518, 0xA519, 0xA51A, 0xA51B, 0xA51C, 0xA51D, 0xA51E, 0xA51F, 0xA520, 0xA521, 0xA522, 0xA523, 0xA524, 0xA525, 0xA526
    };

    [Constructable]
    public VirtueRune() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Virtue Rune";
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

    public VirtueRune(Serial serial) : base(serial)
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
