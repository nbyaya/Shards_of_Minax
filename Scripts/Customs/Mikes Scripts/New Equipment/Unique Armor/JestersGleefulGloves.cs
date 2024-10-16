using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JestersGleefulGloves : StuddedGloves
{
    [Constructable]
    public JestersGleefulGloves()
    {
        Name = "Jester's Gleeful Gloves";
        Hue = Utility.Random(500, 800);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.CastSpeed = 1;
        Attributes.BonusInt = 30;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JestersGleefulGloves(Serial serial) : base(serial)
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
