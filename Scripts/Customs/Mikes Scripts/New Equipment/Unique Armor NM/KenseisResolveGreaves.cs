using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KenseisResolveGreaves : PlateLegs
{
    [Constructable]
    public KenseisResolveGreaves()
    {
        Name = "Kensei's Resolve Greaves";
        Hue = Utility.Random(300, 800);
        BaseArmorRating = Utility.RandomMinMax(65, 95);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusDex = 35;
        Attributes.BonusHits = 20;
        SkillBonuses.SetValues(0, SkillName.Bushido, 40.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 30.0);
        PhysicalBonus = 25;
        FireBonus = 10;
        ColdBonus = 15;
        EnergyBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KenseisResolveGreaves(Serial serial) : base(serial)
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
