using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkFathersDreadnaughtBoots : PlateGorget
{
    [Constructable]
    public DarkFathersDreadnaughtBoots()
    {
        Name = "Dark Father's Dreadnaught Boots";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterPoison = 30;
        ArmorAttributes.SelfRepair = 20;
        Attributes.AttackChance = 15;
        Attributes.BonusDex = 25;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        ColdBonus = 20;
        EnergyBonus = 25;
        FireBonus = 20;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkFathersDreadnaughtBoots(Serial serial) : base(serial)
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
