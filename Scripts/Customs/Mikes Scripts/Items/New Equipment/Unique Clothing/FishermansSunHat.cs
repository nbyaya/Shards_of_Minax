using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FishermansSunHat : WideBrimHat
{
    [Constructable]
    public FishermansSunHat()
    {
        Name = "Fisherman's Sun Hat";
        Hue = Utility.Random(300, 2800);
        ClothingAttributes.LowerStatReq = 2;
        Attributes.BonusInt = 5;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
        Resistances.Energy = 5;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FishermansSunHat(Serial serial) : base(serial)
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
