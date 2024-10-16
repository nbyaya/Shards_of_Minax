using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PaupersPlateGorget : PlateGorget
{
    [Constructable]
    public PaupersPlateGorget()
    {
        Name = "Pauper's Plate Gorget";
        Hue = Utility.Random(100, 200);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        ArmorAttributes.LowerStatReq = 50;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Begging, 40.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 30.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PaupersPlateGorget(Serial serial) : base(serial)
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
