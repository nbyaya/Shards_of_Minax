using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VagrantsVambraces : PlateArms
{
    [Constructable]
    public VagrantsVambraces()
    {
        Name = "Vagrant's Vambraces";
        Hue = Utility.Random(650, 850);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusHits = 30;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.Begging, 45.0);
        SkillBonuses.SetValues(1, SkillName.ArmsLore, 30.0);
        PhysicalBonus = 25;
        FireBonus = 10;
        ColdBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VagrantsVambraces(Serial serial) : base(serial)
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
