using System;
using Server;
using Server.Items;
using Server.Network;

public class MasterShrubbery : Item
{
    private static int[] Graphics = new int[]
    {
        0xA63F, 0xA640, 0xA641, 0xA642, 0xA643, 0xA645, 0xA646, 0xA647, 0xA648, 0xA65C, 0xA65D, 0xA65E, 0xA65F, 0xA660, 0xA661, 0xA662, 0xA663, 0xA664, 0xA665, 0xA666, 0xA667, 0xA668, 0xA669, 0xA66A, 0xA66B, 0xA66C, 0xA66D, 0xA66E, 0xA66F, 0xA670, 0xA671, 0xA676, 0xA677, 0xA678, 0xA679, 0xA67A, 0xA67C, 0xA67D, 0xA67E, 0xA67F, 0xA680, 0xA681
    };

    [Constructable]
    public MasterShrubbery() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "A Shrubbery";
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

    public MasterShrubbery(Serial serial) : base(serial)
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
