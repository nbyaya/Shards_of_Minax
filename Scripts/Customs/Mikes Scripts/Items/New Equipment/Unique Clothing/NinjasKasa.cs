using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NinjasKasa : Kasa
{
    [Constructable]
    public NinjasKasa()
    {
        Name = "Ninja's Kasa";
        Hue = Utility.Random(300, 1400);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusInt = 10;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Ninjitsu, 25.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
        Resistances.Energy = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NinjasKasa(Serial serial) : base(serial)
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
