using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SherwoodArchersCap : LeatherCap
{
    [Constructable]
    public SherwoodArchersCap()
    {
        Name = "Sherwood Archer's Cap";
        Hue = Utility.Random(250, 550);
        BaseArmorRating = Utility.RandomMinMax(20, 40);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.SelfRepair = 3;
        Attributes.BonusDex = 15;
        Attributes.AttackChance = 10;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SherwoodArchersCap(Serial serial) : base(serial)
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
