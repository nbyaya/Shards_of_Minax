using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DruidsProtector : WoodenShield
{
    [Constructable]
    public DruidsProtector()
    {
        Name = "Druid's Protector";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(30, 55);
        ArmorAttributes.ReactiveParalyze = 1;
        ArmorAttributes.SelfRepair = 10;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 50.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 30.0);
        SkillBonuses.SetValues(2, SkillName.Peacemaking, 30.0);
        PhysicalBonus = 10;
        FireBonus = 10;
        ColdBonus = 20;
        EnergyBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DruidsProtector(Serial serial) : base(serial)
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
