using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Stormshield : PlateChest
{
    [Constructable]
    public Stormshield()
    {
        Name = "Stormshield";
        Hue = Utility.Random(450, 850);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusInt = 15;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        ColdBonus = 10;
        EnergyBonus = 25;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Stormshield(Serial serial) : base(serial)
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
