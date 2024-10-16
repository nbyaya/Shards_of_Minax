using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticSeersPlate : PlateChest
{
    [Constructable]
    public MysticSeersPlate()
    {
        Name = "Mystic Seer's Plate";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(30, 80);
        AbsorptionAttributes.EaterEnergy = 30;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 50;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticSeersPlate(Serial serial) : base(serial)
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
