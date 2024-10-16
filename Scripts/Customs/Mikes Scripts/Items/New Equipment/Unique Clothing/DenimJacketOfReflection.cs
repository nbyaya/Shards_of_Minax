using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DenimJacketOfReflection : Tunic
{
    [Constructable]
    public DenimJacketOfReflection()
    {
        Name = "Denim Jacket of Reflection";
        Hue = Utility.Random(500, 2500);
        Attributes.ReflectPhysical = 10;
        Attributes.DefendChance = 7;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        Resistances.Physical = 15;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DenimJacketOfReflection(Serial serial) : base(serial)
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
