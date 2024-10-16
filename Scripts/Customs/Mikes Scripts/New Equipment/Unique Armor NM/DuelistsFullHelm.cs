using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DuelistsFullHelm : PlateHelm
{
    [Constructable]
    public DuelistsFullHelm()
    {
        Name = "Duelist's Full Helm";
        Hue = Utility.Random(4001, 5000);
        BaseArmorRating = Utility.RandomMinMax(55, 85);
        AbsorptionAttributes.EaterKinetic = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusHits = 30;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 45.0);
        SkillBonuses.SetValues(1, SkillName.Fencing, 30.0);
        PhysicalBonus = 15;
        EnergyBonus = 20;
        FireBonus = 15;
        ColdBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DuelistsFullHelm(Serial serial) : base(serial)
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
