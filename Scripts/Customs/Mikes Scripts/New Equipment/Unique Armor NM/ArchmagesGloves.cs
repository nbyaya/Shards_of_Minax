using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArchmagesGloves : LeatherGloves
{
    [Constructable]
    public ArchmagesGloves()
    {
        Name = "Archmage's Gloves";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(10, 50);
        AbsorptionAttributes.CastingFocus = 5;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 20;
        Attributes.RegenMana = 2;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
        PhysicalBonus = 8;
        EnergyBonus = 8;
        FireBonus = 8;
        ColdBonus = 8;
        PoisonBonus = 8;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArchmagesGloves(Serial serial) : base(serial)
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
