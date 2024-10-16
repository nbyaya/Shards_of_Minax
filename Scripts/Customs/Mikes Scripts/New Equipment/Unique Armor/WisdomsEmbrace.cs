using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WisdomsEmbrace : PlateChest
{
    [Constructable]
    public WisdomsEmbrace()
    {
        Name = "Wisdom's Embrace";
        Hue = Utility.Random(400, 600);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 50;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 20;
        EnergyBonus = 25;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WisdomsEmbrace(Serial serial) : base(serial)
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
