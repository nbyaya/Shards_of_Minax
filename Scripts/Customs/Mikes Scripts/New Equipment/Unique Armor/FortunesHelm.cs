using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FortunesHelm : PlateHelm
{
    [Constructable]
    public FortunesHelm()
    {
        Name = "Fortune's Helm";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.SelfRepair = 4;
        Attributes.Luck = 150;
        SkillBonuses.SetValues(0, SkillName.ItemID, 15.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FortunesHelm(Serial serial) : base(serial)
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
