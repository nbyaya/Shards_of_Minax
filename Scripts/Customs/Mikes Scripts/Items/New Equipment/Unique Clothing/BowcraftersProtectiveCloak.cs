using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowcraftersProtectiveCloak : Cloak
{
    [Constructable]
    public BowcraftersProtectiveCloak()
    {
        Name = "Bowcrafter's Protective Cloak";
        Hue = Utility.Random(450, 2450);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Fletching, 25.0);
        Resistances.Energy = 10;
        Resistances.Poison = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowcraftersProtectiveCloak(Serial serial) : base(serial)
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
