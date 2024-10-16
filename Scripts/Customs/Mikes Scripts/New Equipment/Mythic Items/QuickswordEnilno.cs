using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections; // Necessary for ArrayList usage

public class QuickswordEnilno : BaseSword
{
    // Item ID for the sword, you can replace this with the appropriate ID
	public override int OldStrengthReq { get { return 20; } }
	public override int AosMinDamage { get { return 10; } }
	public override int AosMaxDamage { get { return 20; } }
	public override int OldMinDamage { get { return 10; } }
	public override int OldMaxDamage { get { return 20; } }
	public override int InitMinHits { get { return 50; } }
	public override int InitMaxHits { get { return 70; } }


    [Constructable]
    public QuickswordEnilno() : base(0xF60) 
    {
        Weight = 6.0;
        Name = "Quicksword Enilno";
        Hue = 1157; // This is just an example hue, change to desired value
    }

    public QuickswordEnilno(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!IsChildOf(from.Backpack))
        {
            from.SendMessage("The sword must be in your backpack to use its power.");
            return;
        }

        // Find and destroy every MinaxSorceress on the server
        ArrayList toDelete = new ArrayList();

        foreach (Mobile m in World.Mobiles.Values)
        {
            if (m is MinaxSorceress)
            {
                toDelete.Add(m);
            }
        }

        foreach (MinaxSorceress minax in toDelete)
        {
            minax.Delete();
        }

        from.SendMessage("You have vanquished all copies of Minax!");
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

