using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TailorsTouch : StuddedGloves
{
    [Constructable]
    public TailorsTouch()
    {
        Name = "Tailor's Touch";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(40, 85);
        AbsorptionAttributes.CastingFocus = 20;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusDex = 20;
        Attributes.RegenMana = 8;
        SkillBonuses.SetValues(0, SkillName.Tailoring, 30.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TailorsTouch(Serial serial) : base(serial)
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
