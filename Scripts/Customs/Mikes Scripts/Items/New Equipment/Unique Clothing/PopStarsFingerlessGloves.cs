using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PopStarsFingerlessGloves : Bandana
{
    [Constructable]
    public PopStarsFingerlessGloves()
    {
        Name = "Pop Star's Fingerless Gloves";
        Hue = Utility.Random(600, 2600);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusDex = 20;
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PopStarsFingerlessGloves(Serial serial) : base(serial)
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
