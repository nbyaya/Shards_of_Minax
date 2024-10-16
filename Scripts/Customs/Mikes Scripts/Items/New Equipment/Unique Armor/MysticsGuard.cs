using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MysticsGuard : PlateChest
{
    [Constructable]
    public MysticsGuard()
    {
        Name = "Mystic's Guard";
        Hue = Utility.Random(200, 900);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.EaterEnergy = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 40;
        Attributes.CastSpeed = 1;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        ColdBonus = 15;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MysticsGuard(Serial serial) : base(serial)
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
