using System;
using Server;
using Server.Items;
using Server.Network;

public class NameTapestry : Item
{
    private static int[] Graphics = new int[]
    {
        0xA683, 0xA684, 0xA685, 0xA686, 0xA687, 0xA688, 0xA689, 0xA68A, 0xA68B, 0xA68C, 0xA68D, 0xA68E, 0xA68F, 0xA690, 0xA691, 0xA692
    };

    [Constructable]
    public NameTapestry() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Name Tapestry";
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

    public NameTapestry(Serial serial) : base(serial)
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
