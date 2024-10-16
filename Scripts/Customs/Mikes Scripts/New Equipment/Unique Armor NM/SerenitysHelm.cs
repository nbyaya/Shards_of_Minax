using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SerenitysHelm : NorseHelm
{
    [Constructable]
    public SerenitysHelm()
    {
        Name = "Serenity's Helm";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(50, 70);
        ArmorAttributes.SelfRepair = 20;
        Attributes.ReflectPhysical = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 40.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 40.0);
        SkillBonuses.SetValues(2, SkillName.Peacemaking, 40.0);
        PhysicalBonus = 20;
        ColdBonus = 10;
        FireBonus = 10;
        EnergyBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SerenitysHelm(Serial serial) : base(serial)
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
