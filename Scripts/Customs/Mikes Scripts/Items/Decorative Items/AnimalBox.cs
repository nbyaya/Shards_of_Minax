using System;
using Server;
using Server.Items;
using Server.Network;

public class AnimalBox : Item
{
    private static int[] Graphics = new int[]
    {
        0xA3EB, 0xA3EC, 0xA3ED, 0xA3EE, 0xA3EF, 0xA3F0, 0xA3F1, 0xA3F2, 0xA3F3, 0xA3F4, 0xA3F5, 0xA3F6, 0xA3F7, 0xA3F8, 0xA3F9, 0xA3FA
    };

    [Constructable]
    public AnimalBox() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Animal Box";
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

    public AnimalBox(Serial serial) : base(serial)
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
