using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormforgedBoots : PlateLegs
{
    [Constructable]
    public StormforgedBoots()
    {
        Name = "Stormforged Boots";
        Hue = Utility.Random(550, 850);
        BaseArmorRating = Utility.RandomMinMax(35, 65);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.SelfRepair = 4;
        Attributes.NightSight = 1;
        Attributes.BonusStam = 15;
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        EnergyBonus = 15;
        ColdBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormforgedBoots(Serial serial) : base(serial)
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
