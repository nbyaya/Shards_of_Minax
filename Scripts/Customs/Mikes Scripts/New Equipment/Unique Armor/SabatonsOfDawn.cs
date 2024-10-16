using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SabatonsOfDawn : PlateGorget
{
    [Constructable]
    public SabatonsOfDawn()
    {
        Name = "Sabatons of Dawn";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterCold = 30;
        ArmorAttributes.SelfRepair = 10;
        Attributes.ReflectPhysical = 15;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 25.0);
        ColdBonus = 25;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SabatonsOfDawn(Serial serial) : base(serial)
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
