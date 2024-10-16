using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AnvilsGuardLegs : PlateLegs
{
    [Constructable]
    public AnvilsGuardLegs()
    {
        Name = "Anvil's Guard Legs";
        Hue = Utility.Random(200, 300);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.SelfRepair = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStr = 20;
        Attributes.RegenHits = 5;
        Attributes.LowerManaCost = 15;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 40.0);
        SkillBonuses.SetValues(1, SkillName.Carpentry, 30.0);
        PhysicalBonus = 25;
        FireBonus = 10;
        ColdBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AnvilsGuardLegs(Serial serial) : base(serial)
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
