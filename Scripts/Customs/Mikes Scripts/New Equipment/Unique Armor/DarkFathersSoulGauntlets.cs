using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkFathersSoulGauntlets : PlateGloves
{
    [Constructable]
    public DarkFathersSoulGauntlets()
    {
        Name = "Dark Father's Soul Gauntlets";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterEnergy = 30;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenMana = 10;
        Attributes.LowerManaCost = 15;
        SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
        ColdBonus = 20;
        EnergyBonus = 25;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkFathersSoulGauntlets(Serial serial) : base(serial)
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
