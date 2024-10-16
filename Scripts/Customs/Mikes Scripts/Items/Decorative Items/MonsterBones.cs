using System;
using Server;
using Server.Items;
using Server.Network;

public class MonsterBones : Item
{
    private static int[] Graphics = new int[]
    {
        0xABC7, 0xABC8, 0xABC9, 0xABCB, 0xABCC, 0xABCD, 0xABCE, 0xABCF, 0xABD0, 0xABD1, 0xABD2, 0xABD3, 0xABD4, 0xABD6, 0xABD7, 0xABD8, 0xABD9, 0xABDA, 0xABDB, 0xABDC, 0xABDD, 0xABDE, 0xABDF, 0xABE0, 0xABE1, 0xABE2
    };

    [Constructable]
    public MonsterBones() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Monster Bones";
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

    public MonsterBones(Serial serial) : base(serial)
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
