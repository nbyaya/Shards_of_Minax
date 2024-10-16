using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AstartesBattlePlate : PlateChest
{
    [Constructable]
    public AstartesBattlePlate()
    {
        Name = "Astartes Battle Plate";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(30, 90);
        AbsorptionAttributes.EaterKinetic = 30;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusStr = 25;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AstartesBattlePlate(Serial serial) : base(serial)
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
