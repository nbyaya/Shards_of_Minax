using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormforgedGauntlets : PlateGloves
{
    [Constructable]
    public StormforgedGauntlets()
    {
        Name = "Stormforged Gauntlets";
        Hue = Utility.Random(550, 850);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        AbsorptionAttributes.EaterFire = 15;
        ArmorAttributes.LowerStatReq = 15;
        Attributes.WeaponSpeed = 10;
        Attributes.BonusDex = 15;
        SkillBonuses.SetValues(0, SkillName.ArmsLore, 10.0);
        EnergyBonus = 15;
        ColdBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormforgedGauntlets(Serial serial) : base(serial)
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
