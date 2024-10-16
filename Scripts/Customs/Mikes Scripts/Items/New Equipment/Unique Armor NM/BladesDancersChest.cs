using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladesDancersChest : PlateChest
{
    [Constructable]
    public BladesDancersChest()
    {
        Name = "Blade Dancer's Chest";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(75, 85);
        AbsorptionAttributes.EaterKinetic = 25;
        ArmorAttributes.SelfRepair = 20;
        Attributes.BonusStr = 20;
        SkillBonuses.SetValues(0, SkillName.Fencing, 50.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 40.0);
        PhysicalBonus = 20;
        FireBonus = 10;
        ColdBonus = 10;
        EnergyBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladesDancersChest(Serial serial) : base(serial)
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
