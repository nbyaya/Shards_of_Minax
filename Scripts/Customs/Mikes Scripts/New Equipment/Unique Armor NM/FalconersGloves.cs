using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FalconersGloves : LeatherGloves
{
    [Constructable]
    public FalconersGloves()
    {
        Name = "Falconer's Gloves";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(20, 40);
        ArmorAttributes.MageArmor = 1;
        Attributes.AttackChance = 15;
        Attributes.DefendChance = 15;
        Attributes.RegenStam = 3;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 50.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 40.0);
        PhysicalBonus = 10;
        EnergyBonus = 10;
        FireBonus = 20;
        ColdBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FalconersGloves(Serial serial) : base(serial)
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
