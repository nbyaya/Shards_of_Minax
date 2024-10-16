using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShadowGripGloves : LeatherGloves
{
    [Constructable]
    public ShadowGripGloves()
    {
        Name = "Shadow Grip Gloves";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.EaterCold = 10;
        Attributes.WeaponSpeed = 5;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Lockpicking, 10.0);
        SkillBonuses.SetValues(1, SkillName.RemoveTrap, 10.0);
        ColdBonus = 15;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShadowGripGloves(Serial serial) : base(serial)
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
