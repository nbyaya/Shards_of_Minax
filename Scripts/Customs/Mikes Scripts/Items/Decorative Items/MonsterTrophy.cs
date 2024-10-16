using System;
using Server;
using Server.Items;
using Server.Network;

public class MonsterTrophy : Item
{
    private static int[] Graphics = new int[]
    {
        0xAD6F, 0xAD70, 0xAD71, 0xAD72, 0xAD73, 0xAD74, 0xAD75, 0xAD76, 0xA9CA, 0xA9CB, 0xA9CC, 0xA9CD
    };

    [Constructable]
    public MonsterTrophy() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Monster Trophy";
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

    public MonsterTrophy(Serial serial) : base(serial)
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
